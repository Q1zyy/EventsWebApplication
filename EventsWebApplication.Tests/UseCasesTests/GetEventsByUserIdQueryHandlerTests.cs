using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByUserId;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class GetEventsByUserIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetEventsByUserIdQueryHandler _handler;

        public GetEventsByUserIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetEventsByUserIdQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Events_For_Given_UserId()
        {
            // Arrange
            var userId = 1;
            var events = new List<Event>
        {
            new Event { Id = 1, Title = "Event 1" },
            new Event { Id = 2, Title = "Event 2" }
        };

            _userRepositoryMock
                .Setup(repo => repo.GetEventsByUserId(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);

            var query = new GetEventsByUserIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Title == "Event 1");
            result.Should().Contain(e => e.Title == "Event 2");

            _userRepositoryMock.Verify(repo => repo.GetEventsByUserId(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Events_Found()
        {
            // Arrange
            var userId = 1;
            var events = new List<Event>();

            _userRepositoryMock
                .Setup(repo => repo.GetEventsByUserId(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);

            var query = new GetEventsByUserIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _userRepositoryMock.Verify(repo => repo.GetEventsByUserId(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
