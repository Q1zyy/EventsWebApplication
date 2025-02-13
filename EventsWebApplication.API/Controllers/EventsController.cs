using EventsWebApplication.Application.UseCases.EventUseCases.Commands.AddPhotoToEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeletePhotoFromEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByUserId;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithFilters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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
        [Route("all")]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventsWithFilters([FromQuery] GetEventsWithFiltersQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
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
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteEventCommand(id), cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddImageToEvent([FromForm] AddPhotoToEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }    
        
        [HttpDelete]
        [Route("delete-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteImageFromEvent([FromBody] DeletePhotoFromEventCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }

    }
}
