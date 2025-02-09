using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants
{
    public record GetEventParticipantsQuery(int eventId) : IRequest<IEnumerable<User>>;
  
}
