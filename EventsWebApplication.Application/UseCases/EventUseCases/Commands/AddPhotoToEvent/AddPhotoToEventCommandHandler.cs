using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.AddPhotoToEvent
{
    public class AddPhotoToEventCommandHandler : IRequestHandler<AddPhotoToEventCommand>
    {

        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;

        public AddPhotoToEventCommandHandler(IEventRepository eventRepository, IImageService imageService)
        {
            _eventRepository = eventRepository;
            _imageService = imageService;
        }

        public async Task Handle(AddPhotoToEventCommand request, CancellationToken cancellationToken)
        {
            var eventObj = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (eventObj == null)
            {
                throw new NotFoundException("No such event");
            }
            
            var path = await _imageService.SaveImageAsync(request.Image);
            
            if (eventObj.Images != null)
            {
                eventObj.Images.Add(path);
            }
            else
            {
                eventObj.Images = new List<string> { path };
            }

            await _eventRepository.UpdateAsync(eventObj, cancellationToken);
            
        }
    }
}
