using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByUserId
{
    public record GetEventsByUserIdQuery(int Id) : IRequest<IEnumerable<Event>>;
}
