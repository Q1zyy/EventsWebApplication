using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeletePhotoFromEvent
{
    public class DeletePhotoFromEventCommandHandler : IRequestHandler<DeletePhotoFromEventCommand>
    {

        private readonly IEventRepository _eventRepository;

        public DeletePhotoFromEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(DeletePhotoFromEventCommand request, CancellationToken cancellationToken)
        {
            var eventObj = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

            if (eventObj == null)
            {
                throw new Exception("No such event");
            }

            if (eventObj.Images != null)
            {
                eventObj.Images.Remove(request.Image);
            }

            await _eventRepository.UpdateAsync(eventObj, cancellationToken);
        }
    }
}
