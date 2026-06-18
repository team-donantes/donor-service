using System.Text.Json;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.DonorService.Application.Events;
using Donnum.DonorService.Application.Features.Donations.Commands.RegisterDonation;
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
        _subscription = await subscriber.SubscribeAsync(
            options.Value.ToEventSubscriptionOptions(),
            HandleMessageAsync,
            stoppingToken);

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

        var command = new RegisterDonationCommand(
            payload.DonorId,
            payload.DonationRequestId,
            payload.MedicalCenterId,
            payload.DonationDate,
            payload.CreatedAt);

        await mediator.Send(command, cancellationToken);

        logger.LogInformation(
            "Donación registrada desde evento {Topic}. DonorId={DonorId}, DonationRequestId={DonationRequestId}, MessageId={MessageId}, CorrelationId={CorrelationId}.",
            envelope.Topic,
            payload.DonorId,
            payload.DonationRequestId,
            envelope.MessageId,
            envelope.CorrelationId);
    }
}
