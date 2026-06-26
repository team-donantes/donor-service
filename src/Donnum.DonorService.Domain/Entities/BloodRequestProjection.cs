namespace Donnum.DonorService.Domain.Entities;

public sealed class BloodRequestProjection
{
    public Guid Id { get; set; }
    public string RequestType { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
}
