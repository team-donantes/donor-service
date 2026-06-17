using Donnum.DonorService.Application.Features.Donors.Commands.CreateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Commands.UpdateDonorProfile;
using Donnum.DonorService.Application.Features.Donors.Queries.GetDonorProfile;
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
        var command = new UpdateDonorProfileCommand(
            Id: id,
            Street: body.Street,
            City: body.City,
            Province: body.Province,
            Latitude: body.Latitude,
            Longitude: body.Longitude);

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
    [ProducesResponseType(typeof(Application.Features.Donors.Dtos.DonorProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDonorProfile(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDonorProfileQuery(id);
        var dto = await _mediator.Send(query, cancellationToken);
        return Ok(dto);
    }
}

public sealed record UpdateDonorProfileRequest(
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude
);
