using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent
{
    public record UpdateEventCommand(
        int Id,
        string Title,
        string Description,
        DateTime EventDateTime,
        int ParticipantsMaxCount,
        string Place,
        int CategoryId,
        IFormFile Image
    ) : IRequest;
}
