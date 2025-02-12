using AutoMapper;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EventsWebApplication.Tests.UseCasesTests
{

    public class UpdateEventCommandHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateEventCommandHandler _handler;

        public UpdateEventCommandHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _imageServiceMock = new Mock<IImageService>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateEventCommandHandler(
                _eventRepositoryMock.Object,
                _imageServiceMock.Object,
                _mapperMock.Object,
                _categoryRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Update_Event_When_Exists()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Title = "Old Title" };
            var command = new UpdateEventCommand(1, "New Title", "New Desc", DateTime.Now, 10, "New Place", 0, null);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map(command, existingEvent));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(mapper => mapper.Map(command, existingEvent), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Event_Does_Not_Exist()
        {
            // Arrange
            var command = new UpdateEventCommand(1, "New Title", "New Desc", DateTime.Now, 10, "New Place", 0, null);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("No such event");

            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Update_Image_When_Provided()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Title = "Old Title", Images = new List<string>() };
            var mockImage = new Mock<IFormFile>();
            var command = new UpdateEventCommand(1, "New Title", "New Desc", DateTime.Now, 10, "New Place", 0, mockImage.Object);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _imageServiceMock.Setup(service => service.SaveImageAsync(It.IsAny<IFormFile>(), "uploads"))
                .ReturnsAsync("new-image-path.jpg");

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map(command, existingEvent));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().ContainSingle().Which.Should().Be("new-image-path.jpg");
            _imageServiceMock.Verify(service => service.SaveImageAsync(mockImage.Object, "uploads"), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Update_Category_When_Provided()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Title = "Old Title", Category = null };
            var newCategory = new Category { Id = 2, Title = "New Category" };
            var command = new UpdateEventCommand(1, "New Title", "New Desc", DateTime.Now, 10, "New Place", 2, null);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newCategory);

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map(command, existingEvent));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Category.Should().BeEquivalentTo(newCategory);
            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(2, It.IsAny<CancellationToken>()), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Not_Update_Category_When_CategoryId_Is_Zero()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Title = "Old Title", Category = new Category { Id = 1, Title = "Old Category" } };
            var command = new UpdateEventCommand(1, "New Title", "New Desc", DateTime.Now, 10, "New Place", 0, null);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map(command, existingEvent));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Category.Id.Should().Be(1);
            existingEvent.Category.Title.Should().Be("Old Category");
            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}
