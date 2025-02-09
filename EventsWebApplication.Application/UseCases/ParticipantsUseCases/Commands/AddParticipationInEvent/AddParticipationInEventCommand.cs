using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EventsWebApplication.Application.UseCases.ParticipantsUseCases.Commands.AddParticipationInEvent
{
    public record AddParticipationInEventCommand(int EventId, int UserId) : IRequest;
}
