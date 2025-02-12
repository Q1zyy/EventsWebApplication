using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsAll;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class GetEventsAllQueryHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly GetEventsAllQueryHandler _handler;

        public GetEventsAllQueryHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _handler = new GetEventsAllQueryHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_PaginatedList_Of_Events()
        {
            // Arrange
            var events = new List<Event>
        {
            new Event { Id = 1, Title = "Event 1" },
            new Event { Id = 2, Title = "Event 2" }
        };
            var paginatedEvents = new PaginatedList<Event> {
                Items = events,
                CurrentPage =  1,
                TotalPages = 1
            };
            var query = new GetEventsAllQuery(1, 2);

            _eventRepositoryMock
                .Setup(repo => repo.GetAllEvents(It.IsAny<CancellationToken>(), 1, 2))
                .ReturnsAsync(paginatedEvents);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(e => e.Title == "Event 1");
            result.Items.Should().Contain(e => e.Title == "Event 2");

            _eventRepositoryMock.Verify(repo => repo.GetAllEvents(It.IsAny<CancellationToken>(), 1, 2), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_PaginatedList_When_No_Events()
        {
            // Arrange
            var paginatedEvents = new PaginatedList<Event> {
                Items = new List<Event>(),
                CurrentPage = 1,
                TotalPages = 1    
            };
            var query = new GetEventsAllQuery(1, 2);

            _eventRepositoryMock
                .Setup(repo => repo.GetAllEvents(It.IsAny<CancellationToken>(), 1, 2))
                .ReturnsAsync(paginatedEvents);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();

            _eventRepositoryMock.Verify(repo => repo.GetAllEvents(It.IsAny<CancellationToken>(), 1, 2), Times.Once);
        }
    }
}
