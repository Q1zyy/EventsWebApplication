using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {

        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}/participants")]
        public async Task<IActionResult> GetEventParticipants(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventParticipantsQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("by-user/{id:int}")]
        public async Task<IActionResult> GetEventsByUserId(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsByUserIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }

    }
}
