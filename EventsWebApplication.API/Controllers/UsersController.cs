using EventsWebApplication.Application.UseCases.UsersUseCases.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
            return Ok(result);
        }

    }
}
