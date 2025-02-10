using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent
{
    public record DeleteEventCommand(int Id) : IRequest;
}
