using Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Queries.GetDonorBadges;
using Donnum.DonorService.Application.Features.Donors.Queries.GetDonorProfile;
using Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;
using Donnum.DonorService.Application.Features.Donors.Mappers;
using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Application.Features.Donors.Queries.GetDonorReliability;
using Donnum.DonorService.Application.Features.Donors.Commands.RegisterAttendance;
using Donnum.DonorService.Presentation.API.Contracts;
using Donnum.DonorService.Presentation.API.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.DonorService.Presentation.API.Controllers;

[ApiController]
[Route("api/donors")]
[Produces("application/json")]
public class DonorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DonorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}/donations")]
    public async Task<IActionResult> GetDonationHistory([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var donorId))
        {
            return NotFound("El formato del ID del donante no es válido.");
        }

        var result = await _mediator.Send(new GetDonationHistoryQuery(donorId), cancellationToken);

        if (result is null)
        {
            return NotFound($"El donante con ID {id} no existe.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new donor profile.
    /// </summary>
    /// <param name="command">Donor creation payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>201 Created with the location header pointing to the new resource.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateDonorProfile(
        [FromBody] CreateDonorProfileCommand command,
        CancellationToken cancellationToken)
    {
        var donorId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(
            nameof(GetDonorProfile),
            new { id = donorId },
            new { id = donorId });
    }

    /// <summary>
    /// Updates the editable fields of an existing donor profile.
    /// Only address (Street, City, Province) and location (Latitude, Longitude) can be changed.
    /// </summary>
    /// <param name="id">Donor identifier (route).</param>
    /// <param name="body">Update payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>204 No Content on success.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDonorProfile(
        [FromRoute] Guid id,
        [FromBody] UpdateDonorProfileRequest body,
        CancellationToken cancellationToken)
    {
        var command = DonorApiMapper.ToCommand(id, body);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Retrieves the public profile of a donor by their ID.
    /// Sensitive fields (DNI, DateOfBirth) are omitted (RN-08).
    /// </summary>
    /// <param name="id">Donor identifier (route).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with the DonorProfileDto, or 404 if not found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DonorProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDonorProfile(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDonorProfileQuery(id);
        var dto = await _mediator.Send(query, cancellationToken);
        return Ok(dto);
    }

    /// <summary>
    /// Returns all badges earned by a donor.
    /// </summary>
    /// <param name="id">Donor identifier (route).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with the list of badges, or 404 if the donor does not exist.</returns>
    [HttpGet("{id:guid}/badges")]
    [ProducesResponseType(typeof(IReadOnlyList<DonorBadgeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDonorBadges(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDonorBadgesQuery(id);
        var badges = await _mediator.Send(query, cancellationToken);
        return Ok(badges);
    }

    /// <summary>
    /// Retrieves the reliability score details for a specific donor.
    /// </summary>
    /// <param name="id">Donor identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with reliability details.</returns>
    [HttpGet("{id:guid}/reliability")]
    [ProducesResponseType(typeof(DonorReliabilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDonorReliability(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDonorReliabilityQuery(id);
        var reliability = await _mediator.Send(query, cancellationToken);
        return Ok(reliability);
    }

    /// <summary>
    /// Registers the attendance of a donor to a donation center and updates their reliability score.
    /// If attended is true, it publishes a donor.donation.registered event.
    /// </summary>
    /// <param name="id">Donor identifier.</param>
    /// <param name="request">Attendance registration request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>204 No Content on success.</returns>
    [HttpPost("{id:guid}/attendance")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterAttendance(
        [FromRoute] Guid id,
        [FromBody] RegisterAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterAttendanceCommand(
            id,
            request.DonationRequestId,
            request.MedicalCenterId,
            request.DonationDate,
            request.Attended
        );

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
