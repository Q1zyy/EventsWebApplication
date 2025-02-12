using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeletePhotoFromEvent;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class DeletePhotoFromEventCommandHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly DeletePhotoFromEventCommandHandler _handler;

        public DeletePhotoFromEventCommandHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _handler = new DeletePhotoFromEventCommandHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Remove_Photo_When_Event_Exists_And_Photo_Is_In_List()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = new List<string> { "image1.jpg", "image2.jpg" } };
            var command = new DeletePhotoFromEventCommand(1, "image1.jpg");

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().NotContain("image1.jpg");
            existingEvent.Images.Should().ContainSingle().Which.Should().Be("image2.jpg");
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Event_Does_Not_Exist()
        {
            // Arrange
            var command = new DeletePhotoFromEventCommand(1, "image1.jpg");

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
        public async Task Handle_Should_Not_Update_Event_When_Photo_Not_In_List()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = new List<string> { "image2.jpg" } };
            var command = new DeletePhotoFromEventCommand(1, "image1.jpg");

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().ContainSingle().Which.Should().Be("image2.jpg");
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Not_Throw_When_Event_Has_No_Images()
        {
            // Arrange
            var existingEvent = new Event { Id = 1, Images = null };
            var command = new DeletePhotoFromEventCommand(1, "image1.jpg");

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingEvent.Images.Should().BeNull();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
