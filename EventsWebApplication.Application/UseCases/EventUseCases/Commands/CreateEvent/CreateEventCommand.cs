using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent
{
    public record CreateEventCommand(
        string Title,
        string Description,
        DateTime EventDateTime,
        int ParticipantsMaxCount,
        string Place,
        Category Category
    ) : IRequest;

}
