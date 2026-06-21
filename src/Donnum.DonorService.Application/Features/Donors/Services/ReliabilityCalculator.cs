using System;
using System.Collections.Generic;
using System.Linq;
using Donnum.DonorService.Domain.Entities;
using Donnum.DonorService.Domain.Enums;

namespace Donnum.DonorService.Application.Features.Donors.Services;

public sealed class ReliabilityCalculator : IReliabilityCalculator
{
    private const int BaseScore = 100;
    private const int MinScore = 0;
    private const int MaxScore = 100;
    
    private const int AbsencePenalty = 20;
    private const int UnsafePenalty = 30;
    private const int SafeReward = 10;
    
    private const int MinimumIntervalDays = 56; // 8 weeks
    private const int MaleYearlyLimit = 4;
    private const int FemaleYearlyLimit = 3;

    public int CalculateNewScore(int currentScore, Donor donor, IReadOnlyList<Donation> donations, DateTime donationDate, bool attended)
    {
        if (!attended)
        {
            return Math.Max(MinScore, currentScore - AbsencePenalty);
        }

        var oneYearAgo = donationDate.AddDays(-365);
        var recentDonationsCount = donations.Count(d => d.DonationDate >= oneYearAgo);

        bool exceededLimits = donor.Gender == Gender.Male ? recentDonationsCount >= MaleYearlyLimit : recentDonationsCount >= FemaleYearlyLimit;

        var lastDonation = donations.OrderByDescending(d => d.DonationDate).FirstOrDefault();
        bool violatesInterval = lastDonation != null && (donationDate - lastDonation.DonationDate).TotalDays < MinimumIntervalDays;

        if (exceededLimits || violatesInterval)
        {
            return Math.Max(MinScore, currentScore - UnsafePenalty);
        }

        return Math.Min(MaxScore, currentScore + SafeReward);
    }
}
