using System;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Domain.Entities;

public class Badge
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public BadgeType BadgeType { get; set; }
    public int BadgePoints { get; set; }
}
