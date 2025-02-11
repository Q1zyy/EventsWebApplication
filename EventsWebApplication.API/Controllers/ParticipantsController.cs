using EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.AddParticipationInEvent;
using EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.RemoveParticipationInEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : Controller
    {

        private readonly IMediator _mediator;

        public ParticipantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipationInEvent(AddParticipationInEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        } 
        
        [HttpDelete]
        public async Task<IActionResult> RemoveParticipationInEvent(RemoveParticipationInEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }
        
    }
}
