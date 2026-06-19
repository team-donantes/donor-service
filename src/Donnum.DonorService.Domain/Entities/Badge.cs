using System;
using Donnum.DonorService.Domain.Enums;

using Donnum.DonorService.Domain.Common;

namespace Donnum.DonorService.Domain.Entities;

public class Badge : Entity
{
    public string Name { get; set; } = string.Empty;
    public BadgeType BadgeType { get; set; }
    public int BadgePoints { get; set; }
}
