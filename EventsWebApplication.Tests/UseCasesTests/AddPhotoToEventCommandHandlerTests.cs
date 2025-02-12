using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.AddPhotoToEvent;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EventsWebApplication.Tests.UseCasesTests
{

    public class AddPhotoToEventCommandHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly AddPhotoToEventCommandHandler _handler;

        public AddPhotoToEventCommandHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _imageServiceMock = new Mock<IImageService>();

            _handler = new AddPhotoToEventCommandHandler(
                _eventRepositoryMock.Object,
                _imageServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Add_Photo_To_Event_When_Event_Exists()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = new List<string>() };
            var mockImage = new Mock<IFormFile>();
            var command = new AddPhotoToEventCommand(1, mockImage.Object);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _imageServiceMock.Setup(service => service.SaveImageAsync(It.IsAny<IFormFile>(), "uploads"))
                .ReturnsAsync("new-image-path.jpg");

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().ContainSingle().Which.Should().Be("new-image-path.jpg");
            _imageServiceMock.Verify(service => service.SaveImageAsync(mockImage.Object, "uploads"), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Event_Does_Not_Exist()
        {
            // Arrange
            var mockImage = new Mock<IFormFile>();
            var command = new AddPhotoToEventCommand(1, mockImage.Object);

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
        public async Task Handle_Should_Create_Image_List_When_Null()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = null };
            var mockImage = new Mock<IFormFile>();
            var command = new AddPhotoToEventCommand(1, mockImage.Object);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _imageServiceMock.Setup(service => service.SaveImageAsync(It.IsAny<IFormFile>(), "uploads"))
                .ReturnsAsync("new-image-path.jpg");

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().ContainSingle().Which.Should().Be("new-image-path.jpg");
            existingEvent.Images.Should().NotBeNull();
            _imageServiceMock.Verify(service => service.SaveImageAsync(mockImage.Object, "uploads"), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Add_To_Existing_Image_List()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = new List<string> { "old-image.jpg" } };
            var mockImage = new Mock<IFormFile>();
            var command = new AddPhotoToEventCommand(1, mockImage.Object);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _imageServiceMock.Setup(service => service.SaveImageAsync(It.IsAny<IFormFile>(), "uploads"))
                .ReturnsAsync("new-image-path.jpg");

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().HaveCount(2);
            existingEvent.Images.Should().Contain(new[] { "old-image.jpg", "new-image-path.jpg" });
            _imageServiceMock.Verify(service => service.SaveImageAsync(mockImage.Object, "uploads"), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
