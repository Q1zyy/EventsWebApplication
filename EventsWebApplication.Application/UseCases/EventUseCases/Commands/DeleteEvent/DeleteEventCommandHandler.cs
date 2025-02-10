using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {

        private readonly IEventRepository _eventRepository;

        public DeleteEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            await _eventRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
