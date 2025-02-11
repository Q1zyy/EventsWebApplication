using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsAll
{
    public class GetEventsAllQueryHandler : IRequestHandler<GetEventsAllQuery, PaginatedList<Event>>
    {

        private readonly IEventRepository _eventRepository;

        public GetEventsAllQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<PaginatedList<Event>> Handle(GetEventsAllQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetAllEvents(cancellationToken, request.PageNo, request.PageSize);
        }
    }
}
