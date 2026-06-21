using System;
using System.Collections.Generic;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donors.Services;

public interface IReliabilityCalculator
{
    int CalculateNewScore(int currentScore, Gender gender, IReadOnlyList<Donation> donations, DateTime donationDate, bool attended);
}
