using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Event>
    {

        private readonly IEventRepository _eventRepository;
        public GetEventByIdQueryHandler(IEventRepository eventRepository) 
        {
           _eventRepository = eventRepository;
        }

        public async Task<Event> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventObj = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

            if (eventObj == null)
            {
                throw new Exception($"Event with ID {request.Id} not found");
            }
            
            return eventObj;
        }
    }
}
