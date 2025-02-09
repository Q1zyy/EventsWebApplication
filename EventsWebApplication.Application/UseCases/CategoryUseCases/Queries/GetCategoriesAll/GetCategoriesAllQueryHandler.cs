using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoriesAll
{
    public class GetCategoriesAllQueryHandler : IRequestHandler<GetCategoriesAllQuery, IEnumerable<Category>>
    {

        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesAllQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> Handle(GetCategoriesAllQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllAsync(cancellationToken);
        }
    }
}
