using Donnum.DonorService.Application.Features.Donations.Queries.GetDonationHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.DonorService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonorsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}/donations")]
    public async Task<IActionResult> GetDonationHistory([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var donorId))
        {
            return NotFound("El formato del ID del donante no es válido.");
        }

        var result = await mediator.Send(new GetDonationHistoryQuery(donorId), cancellationToken);

        if (result is null)
        {
            return NotFound($"El donante con ID {id} no existe.");
        }

        return Ok(result);
    }
}
