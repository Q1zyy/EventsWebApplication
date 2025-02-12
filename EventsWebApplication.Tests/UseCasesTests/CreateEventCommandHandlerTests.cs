using AutoMapper;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{

    public class CreateEventCommandHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateEventCommandHandler _handler;

        public CreateEventCommandHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _imageServiceMock = new Mock<IImageService>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateEventCommandHandler(
                _eventRepositoryMock.Object,
                _imageServiceMock.Object,
                _mapperMock.Object,
                _categoryRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_SaveImage_And_CreateEvent()
        {
            // Arrange
            var testImage = new Mock<IFormFile>(); 
            var command = new CreateEventCommand(
                "Test Event",
                "Description",
                DateTime.UtcNow,
                100,
                "Test Place",
                1,
                testImage.Object
            );

            var testCategory = new Category { Id = 1, Title = "Test Category" };
            var testEvent = new Event { Id = 1, Title = "Test Event" };

            _imageServiceMock
                .Setup(service => service.SaveImageAsync(It.IsAny<IFormFile>(), "uploads"))
                .ReturnsAsync("test/path/image.jpg");

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testCategory);

            _mapperMock
                .Setup(mapper => mapper.Map<Event>(It.IsAny<CreateEventCommand>()))
                .Returns(testEvent);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _imageServiceMock.Verify(service => service.SaveImageAsync(testImage.Object, "uploads"), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenCategoryNotFound()
        {
            // Arrange
            var command = new CreateEventCommand(
                "Test Event",
                "Description",
                DateTime.UtcNow,
                100,
                "Test Place",
                999,
                new Mock<IFormFile>().Object
            );

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }

}
