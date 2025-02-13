using System.Diagnostics;
using System.Security.Claims;
using EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.AddParticipationInEvent;
using EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.RemoveParticipationInEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AddParticipationInEvent(AddParticipationInEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        } 
        
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveParticipationInEvent(RemoveParticipationInEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }
        
    }
}
