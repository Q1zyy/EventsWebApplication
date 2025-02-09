using EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoriesAll;
using EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoriesAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoriesAllQuery(), cancellationToken);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
            return Ok(result);
        }


    }
}
