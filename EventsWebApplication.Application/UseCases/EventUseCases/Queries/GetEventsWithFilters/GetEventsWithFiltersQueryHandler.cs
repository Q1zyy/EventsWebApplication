using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithFilters
{
    public class GetEventsWithFiltersQueryHandler : IRequestHandler<GetEventsWithFiltersQuery, PaginatedList<Event>>
    {

        private readonly IEventRepository _eventRepository;

        public GetEventsWithFiltersQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<PaginatedList<Event>> Handle(GetEventsWithFiltersQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetEventsWithFilters(cancellationToken,
                request.Title,
                request.DateFrom,
                request.DateTo,
                request.Place,
                request.CategoryId,
                request.pageNo,
                request.pageSize
             );
        }
    }
}
