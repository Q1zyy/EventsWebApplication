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
        public async Task<IActionResult> AddParticipationInEvent(int eventId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddParticipationInEventCommand(eventId, 1), cancellationToken);
            return Ok();
        } 
        
        [HttpDelete]
        public async Task<IActionResult> RemoveParticipationInEvent(int eventId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveParticipationInEventCommand(eventId, 1), cancellationToken);
            return Ok();
        }
        
    }
}
