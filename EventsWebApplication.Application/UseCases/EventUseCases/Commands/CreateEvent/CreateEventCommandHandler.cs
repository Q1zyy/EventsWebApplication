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

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
    {

        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(
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

        public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var path = await _imageService.SaveImageAsync(request.Image);
            var eventObj = _mapper.Map<Event>(request);
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            eventObj.Category = category;
            eventObj.Images = new List<string> { path };
            await _eventRepository.AddAsync(eventObj, cancellationToken);
        }
    }
}
