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
    public class GetEventParticipantsQueryHandler : IRequestHandler<GetEventParticipantsQuery, IEnumerable<Participant>>
    {

        private readonly IParticipantRepository _participantRepository;

        public GetEventParticipantsQueryHandler(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<IEnumerable<Participant>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
        {
            return await _participantRepository.GetEventParticipants(request.eventId, cancellationToken);
        }
    }
}
