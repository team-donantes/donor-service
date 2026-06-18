using Donnum.BuildingBlocks.Messaging.Abstractions;

namespace Donnum.DonorService.Infrastructure.Messaging;

public sealed class DonationCompletedEventSubscriptionOptions
{
    public const string SectionName = "Messaging:DonationCompletedEvents";

    public string QueueName { get; init; } = "donor-service.donation-completed";

    public string RoutingKey { get; init; } = "donation.physical.completed";

    public ushort PrefetchCount { get; init; } = 16;

    public bool RequeueOnError { get; init; } = true;

    public EventSubscriptionOptions ToEventSubscriptionOptions()
        => new(QueueName, RoutingKey)
        {
            PrefetchCount = PrefetchCount,
            RequeueOnError = RequeueOnError
        };
}
