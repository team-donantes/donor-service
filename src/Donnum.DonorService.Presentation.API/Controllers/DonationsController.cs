using Donnum.DonorService.Application.Features.Donations.Commands.CancelParticipation;
using Donnum.DonorService.Application.Features.Donations.Commands.CreateParticipation;
using Donnum.DonorService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.DonorService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DonationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DonationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{donorId:guid}/participation")]
    public async Task<IActionResult> CreateParticipation([FromRoute] Guid donorId, [FromBody] CreateParticipationRequest request, CancellationToken cancellationToken)
    {
        if (donorId != request.DonorId)
        {
            return BadRequest("DonorId in route does not match DonorId in payload.");
        }

        var command = new CreateParticipationCommand(donorId, request.DonationRequestId, request.Status, request.RequestedBloodTypes);
        await _mediator.Send(command, cancellationToken);
        
        return Ok();
    }

    [HttpPatch("{donorId:guid}/participation/{requestId:guid}")]
    public async Task<IActionResult> CancelParticipation([FromRoute] Guid donorId, [FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        var command = new CancelParticipationCommand(donorId, requestId);
        await _mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}

public record CreateParticipationRequest(Guid DonorId, Guid DonationRequestId, ParticipationStatus Status, List<string> RequestedBloodTypes);
