using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants;
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

    }
}
