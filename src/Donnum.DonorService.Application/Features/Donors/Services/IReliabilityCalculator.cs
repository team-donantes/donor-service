using System;
using System.Collections.Generic;
using Donnum.DonorService.Domain.Entities;

namespace Donnum.DonorService.Application.Features.Donors.Services;

public interface IReliabilityCalculator
{
    int CalculateNewScore(int currentScore, Donor donor, IReadOnlyList<Donation> donations, DateTime donationDate, bool attended);
}
