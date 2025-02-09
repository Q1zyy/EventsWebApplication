using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants
{
    public class GetEventParticipantsQueryHandler : IRequestHandler<GetEventParticipantsQuery, IEnumerable<User>>
    {

        private readonly IEventRepository _eventRepository;

        public GetEventParticipantsQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetEventParticipants(request.eventId, cancellationToken);
        }
    }
}
