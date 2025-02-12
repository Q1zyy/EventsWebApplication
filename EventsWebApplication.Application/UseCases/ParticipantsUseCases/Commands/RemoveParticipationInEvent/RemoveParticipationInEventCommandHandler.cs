using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.RemoveParticipationInEvent
{
    public class RemoveParticipationInEventCommandHandler : IRequestHandler<RemoveParticipationInEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParticipantRepository _participantRepository;

        public RemoveParticipationInEventCommandHandler(
            IEventRepository eventRepository,
            IUserRepository userRepository,
            IParticipantRepository participantRepository
        )
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _participantRepository = participantRepository;
        }

        public async Task Handle(RemoveParticipationInEventCommand request, CancellationToken cancellationToken)
        {

            var eventObj = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (eventObj == null)
            {
                throw new NotFoundException($"Event with ID {request.EventId} not found");
            }

            if (user == null)
            {
                throw new NotFoundException("No such user");
            }

            if (!user.Events.Contains(eventObj))
            {
                throw new BadRequestException($"Users doesnt participate in this event");
            }

            var participant = await _participantRepository.GetParticipantByEventAndUserId(request.EventId, request.UserId, cancellationToken);

            if (participant == null)
            {
                throw new NotFoundException("No such participant");
            }

            await _participantRepository.DeleteAsync(participant.Id, cancellationToken);
            user.Events.Remove(eventObj);
            eventObj.Participants.Remove(participant);
            await _userRepository.ConfirmChanges(cancellationToken);
            await _eventRepository.ConfirmChanges(cancellationToken);

        }
    }
}
