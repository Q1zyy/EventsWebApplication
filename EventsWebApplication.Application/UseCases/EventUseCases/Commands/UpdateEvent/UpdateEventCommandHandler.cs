using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {

        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateEventCommandHandler(
            IEventRepository eventRepository,
            IImageService imageService,
            IMapper mapper,
            ICategoryRepository categoryRepository
        )
        {
            _eventRepository = eventRepository;
            _imageService = imageService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventObj = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

            if (eventObj == null)
            {
                throw new Exception("No such event");
            }
            
            _mapper.Map(request, eventObj); 
            
            if (request.Image != null)
            {
                var path = await _imageService.SaveImageAsync(request.Image);
                eventObj.Images = new List<string> { path };
            }

            if (request.CategoryId != 0)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
                eventObj.Category = category;
            }

            await _eventRepository.UpdateAsync(eventObj, cancellationToken);

        }
    }
}
