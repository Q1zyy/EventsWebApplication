using EventsWebApplication.Application.UseCases.AuthUseCases.Commands.LoginUser;
using EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RefreshToken;
using EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }   
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _mediator.Send(request, cancellationToken);
            return Ok(token);
        }

    }
}
