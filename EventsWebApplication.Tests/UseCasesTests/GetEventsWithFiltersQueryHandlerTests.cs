using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithFilters;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class GetEventsWithFiltersQueryHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly GetEventsWithFiltersQueryHandler _handler;

        public GetEventsWithFiltersQueryHandlerTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _handler = new GetEventsWithFiltersQueryHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Filtered_Events()
        {
            // Arrange
            var query = new GetEventsWithFiltersQuery(
                Title: "Event 1",
                DateFrom: new DateTime(2025, 1, 1),
                DateTo: new DateTime(2025, 12, 31),
                Place: "New York",
                CategoryId: 1,
                pageNo: 1,
                pageSize: 5
            );

            var events = new List<Event>
        {
            new Event { Id = 1, Title = "Event 1", Place = "New York", EventDateTime = new DateTime(2025, 5, 1) },
            new Event { Id = 2, Title = "Event 2", Place = "New York", EventDateTime = new DateTime(2025, 6, 1) }
        };

            var paginatedList = new PaginatedList<Event>
            {
                Items = events,
                TotalPages = 1,
                CurrentPage = 1
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventsWithFilters(
                    It.IsAny<CancellationToken>(),
                    query.Title,
                    query.DateFrom,
                    query.DateTo,
                    query.Place,
                    query.CategoryId,
                    query.pageNo,
                    query.pageSize
                ))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(e => e.Title == "Event 1");
            result.Items.Should().Contain(e => e.Place == "New York");
            result.TotalPages.Should().Be(1);
            result.CurrentPage.Should().Be(1);

            _eventRepositoryMock.Verify(repo => repo.GetEventsWithFilters(It.IsAny<CancellationToken>(),
                query.Title, query.DateFrom, query.DateTo, query.Place, query.CategoryId, query.pageNo, query.pageSize), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Events_Match_Filters()
        {
            // Arrange
            var query = new GetEventsWithFiltersQuery(
                Title: "Nonexistent Event",
                DateFrom: new DateTime(2025, 1, 1),
                DateTo: new DateTime(2025, 12, 31),
                Place: "Unknown",
                CategoryId: 99,
                pageNo: 1,
                pageSize: 5
            );

            var events = new List<Event>();

            var paginatedList = new PaginatedList<Event>
            {
                Items = events,
                TotalPages = 0,
                CurrentPage = 1
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventsWithFilters(
                    It.IsAny<CancellationToken>(),
                    query.Title,
                    query.DateFrom,
                    query.DateTo,
                    query.Place,
                    query.CategoryId,
                    query.pageNo,
                    query.pageSize
                ))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalPages.Should().Be(0);
            result.CurrentPage.Should().Be(1);

            _eventRepositoryMock.Verify(repo => repo.GetEventsWithFilters(It.IsAny<CancellationToken>(),
                query.Title, query.DateFrom, query.DateTo, query.Place, query.CategoryId, query.pageNo, query.pageSize), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Events_With_Default_Pagination_If_No_Page_Params()
        {
            // Arrange
            var query = new GetEventsWithFiltersQuery(
                Title: "Event",
                null,
                null,
                null,
                null,
                pageNo: 1,
                pageSize: 2
            );

            var events = new List<Event>
        {
            new Event { Id = 1, Title = "Event 1" },
            new Event { Id = 2, Title = "Event 2" }
        };

            var paginatedList = new PaginatedList<Event>
            {
                Items = events,
                TotalPages = 1,
                CurrentPage = 1
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventsWithFilters(
                    It.IsAny<CancellationToken>(),
                    query.Title,
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    query.pageNo,
                    query.pageSize
                ))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(e => e.Title == "Event 1");
            result.Items.Should().Contain(e => e.Title == "Event 2");
            result.TotalPages.Should().Be(1);
            result.CurrentPage.Should().Be(1);

            _eventRepositoryMock.Verify(repo => repo.GetEventsWithFilters(It.IsAny<CancellationToken>(),
                query.Title, It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<int?>(),
                query.pageNo, query.pageSize), Times.Once);
        }
    }
}
