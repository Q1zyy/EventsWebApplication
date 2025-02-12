using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.AddParticipationInEvent
{
    public class AddParticipationInEventCommandHandler : IRequestHandler<AddParticipationInEventCommand>
    {

        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParticipantRepository _participantRepository;

        public AddParticipationInEventCommandHandler(
            IEventRepository eventRepository,
            IUserRepository userRepository,
            IParticipantRepository participantRepository
        ) {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _participantRepository = participantRepository;
        }

        public async Task Handle(AddParticipationInEventCommand request, CancellationToken cancellationToken)
        {
            var eventObj = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (eventObj == null)
            {
                throw new NotFoundException($"Event with ID {request.EventId} not found");
            }

            if (eventObj.Participants.Count >= eventObj.ParticipantsMaxCount)
            {
                throw new BadRequestException($"No more slots to this Event");
            }

            if (user == null)
            {
                throw new NotFoundException("No such user");
            }

            if (user.Events.Contains(eventObj))
            {
                throw new BadRequestException($"Already participate");
            }

            var participant = new Participant() {
                EventId = eventObj.Id,
                UserId = user.Id,
                RegistrationTime = DateTime.UtcNow,
                User = user 
            };

            await _participantRepository.AddAsync(participant, cancellationToken);
            user.Events.Add(eventObj);
            eventObj.Participants.Add(participant);
            await _userRepository.ConfirmChanges(cancellationToken);
            await _eventRepository.ConfirmChanges(cancellationToken);
        }
    }
}
