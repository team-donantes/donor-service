using Donnum.DonorService.Application.Features.Donors.Dtos;
using Donnum.DonorService.Application.Features.Donors.Queries.GetDonorByAuthUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.DonorService.Presentation.API.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the donor ID of a donor by their auth user ID.
    /// </summary>
    /// <param name="userId">Auth User identifier (route).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with the Guid donor ID, or 404 if not found.</returns>
    [HttpGet("{userId:guid}/donor")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDonorByUserId(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetDonorByAuthUserIdQuery(userId);
        var dto = await _mediator.Send(query, cancellationToken);
        return Ok(dto);
    }
}
