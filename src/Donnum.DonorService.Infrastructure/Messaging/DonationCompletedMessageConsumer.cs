using System.Text.Json;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.DonorService.Application.Events;
using Donnum.DonorService.Application.Features.Donations.Events.DonationCompleted;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Donnum.DonorService.Infrastructure.Messaging;

public sealed class DonationCompletedMessageConsumer(
    IMessageSubscriber subscriber,
    IServiceScopeFactory scopeFactory,
    IOptions<DonationCompletedEventSubscriptionOptions> options,
    ILogger<DonationCompletedMessageConsumer> logger) : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private IEventSubscription? _subscription;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
        _subscription = await subscriber.SubscribeAsync(
            options.Value.ToEventSubscriptionOptions(),
            HandleMessageAsync,
            stoppingToken);
        }
        catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
        {
            Console.WriteLine($"[WARNING] No se pudo conectar a RabbitMQ al iniciar: {ex.Message}");
        }

        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscription is not null)
        {
            await _subscription.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }

    private async Task HandleMessageAsync(MessageEnvelope envelope, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Deserialize<DonorRegisterDonation>(envelope.Payload, JsonOptions)
            ?? throw new JsonException("El payload del evento de donación no pudo ser deserializado.");

        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var @event = new DonationCompletedEvent(
            payload.DonorId,
            payload.DonationRequestId,
            payload.MedicalCenterId,
            payload.DonationDate,
            payload.CreatedAt);

        await mediator.Send(@event, cancellationToken);

        // TODO: En el futuro, persistir esta información en una tabla de auditoría dedicada si se requiere.
        logger.LogInformation(
            "Donación registrada. DonorId: {DonorId}, DonationRequestId: {DonationRequestId}",
            payload.DonorId,
            payload.DonationRequestId);
    }
}
