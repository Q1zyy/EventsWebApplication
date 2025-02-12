using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;
using FluentAssertions;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Tests.UseCasesTests
{

    public class DeleteEventCommandHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly DeleteEventCommandHandler _handler;

        public DeleteEventCommandHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _handler = new DeleteEventCommandHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Call_DeleteAsync()
        {
            // Arrange
            var command = new DeleteEventCommand(1);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_NotThrowException_When_EventDoesNotExist()
        {
            // Arrange
            var command = new DeleteEventCommand(999); 

            _eventRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask); 

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync<Exception>();
        }
    }

}
