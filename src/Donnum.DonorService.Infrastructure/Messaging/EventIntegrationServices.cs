using System.Text.Json;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.BuildingBlocks.Messaging.Constants;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Donnum.DonorService.Infrastructure.Messaging;

public sealed class IntegrationEventOutbox(ApplicationDbContext db) : IIntegrationEventOutbox
{
    public Task EnqueueAsync(string exchangeName, string topic, object payload, string? correlationId = null, Guid? messageId = null, CancellationToken cancellationToken = default)
    {
        db.OutboxMessages.Add(new OutboxMessage
        {
            Id = messageId ?? Guid.NewGuid(),
            ExchangeName = exchangeName,
            Topic = topic,
            CorrelationId = correlationId ?? Guid.NewGuid().ToString("D"),
            Payload = IntegrationEventEnvelope.SerializePayload(payload)
        });
        return Task.CompletedTask;
    }
}

public sealed class OutboxPublisherWorker(IServiceScopeFactory scopeFactory, IEventBus eventBus, ILogger<OutboxPublisherWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        do { await PublishBatchAsync(stoppingToken); }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    internal async Task PublishBatchAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var now = DateTimeOffset.UtcNow;
        var messages = await db.OutboxMessages.Where(x => x.PublishedAt == null && x.NextAttemptAt <= now)
            .OrderBy(x => x.OccurredAt).Take(50).ToListAsync(cancellationToken);
        foreach (var message in messages)
        {
            try
            {
                await eventBus.PublishAsync(message.ExchangeName, IntegrationEventEnvelope.Create(message), cancellationToken);
                message.PublishedAt = DateTimeOffset.UtcNow;
                message.LastError = null;
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
            {
                message.Attempts++;
                message.LastError = exception.Message.Length <= 2000 ? exception.Message : exception.Message[..2000];
                message.NextAttemptAt = DateTimeOffset.UtcNow.AddSeconds(Math.Min(300, Math.Pow(2, message.Attempts)));
                logger.LogError(exception, "Failed to publish outbox message {MessageId}.", message.Id);
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}

public sealed class BloodRequestCreatedConsumer(IMessageSubscriber subscriber, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private IEventSubscription? _subscription;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscription = await subscriber.SubscribeAsync(
            new EventSubscriptionOptions("donor-service.request.bloodrequest.created", DonnumEventTopics.RequestBloodRequestCreated)
            {
                ExchangeName = Exchanges.Request,
                RequeueOnError = false,
                DeadLetter = new DeadLetterSubscriptionOptions(
                    Exchanges.DeadLetters,
                    "donor-service.request.bloodrequest.created.dlq",
                    "donor-service.request.bloodrequest.created.dead-letter")
            },
            HandleAsync,
            stoppingToken);
        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }

    private async Task HandleAsync(MessageEnvelope envelope, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await db.InboxMessages.AnyAsync(x => x.MessageId == envelope.MessageId, cancellationToken)) return;
        var payload = JsonSerializer.Deserialize<BloodRequestCreatedPayload>(
            envelope.Payload,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new JsonException("Invalid blood request payload.");
        db.InboxMessages.Add(new InboxMessage
        {
            MessageId = envelope.MessageId,
            Topic = envelope.Topic,
            ProcessedAt = DateTimeOffset.UtcNow
        });
        var projection = await db.BloodRequestProjections.FindAsync([payload.RequestId], cancellationToken);
        if (projection is null)
        {
            db.BloodRequestProjections.Add(new BloodRequestProjection
            {
                Id = payload.RequestId,
                RequestType = payload.RequestType,
                OccurredAt = payload.OccurredAt
            });
        }
        else
        {
            projection.RequestType = payload.RequestType;
            projection.OccurredAt = payload.OccurredAt;
        }
        await db.SaveChangesAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscription is not null) await _subscription.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }

    private sealed record BloodRequestCreatedPayload(Guid EventId, DateTimeOffset OccurredAt, Guid RequestId, string RequestType);
}
