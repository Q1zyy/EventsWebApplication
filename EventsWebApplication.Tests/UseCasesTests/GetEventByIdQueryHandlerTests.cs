using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class GetEventByIdQueryHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly GetEventByIdQueryHandler _handler;

        public GetEventByIdQueryHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _handler = new GetEventByIdQueryHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Event_When_Event_Exists()
        {
            // Arrange
            var expectedEvent = new Event { Id = 1, Title = "Test Event" };
            var query = new GetEventByIdQuery(1);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEvent);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Test Event");

            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Event_Does_Not_Exist()
        {
            // Arrange
            var query = new GetEventByIdQuery(99);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Event with ID 99 not found");

            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(99, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
