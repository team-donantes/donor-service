using System;
using MediatR;

namespace Donnum.DonorService.Application.Features.Donors.Commands.RegisterAttendance;

public sealed record RegisterAttendanceCommand(
    Guid DonorId,
    Guid DonationRequestId,
    Guid MedicalCenterId,
    DateTime DonationDate,
    bool Attended
) : IRequest;
