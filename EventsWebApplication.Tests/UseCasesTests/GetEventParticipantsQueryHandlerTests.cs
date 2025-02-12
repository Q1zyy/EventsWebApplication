using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventParticipants;
using EventsWebApplication.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.UseCasesTests
{
    public class GetEventParticipantsQueryHandlerTests
    {
        private readonly Mock<IParticipantRepository> _participantRepositoryMock;
        private readonly GetEventParticipantsQueryHandler _handler;

        public GetEventParticipantsQueryHandlerTests()
        {
            _participantRepositoryMock = new Mock<IParticipantRepository>();
            _handler = new GetEventParticipantsQueryHandler(_participantRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Participants_When_Event_Has_Participants()
        {
            // Arrange
            var participants = new List<Participant>
        {
            new Participant { Id = 1, UserId = 1 },
            new Participant { Id = 2, UserId = 2 }
        };
            var query = new GetEventParticipantsQuery(1);

            _participantRepositoryMock.Setup(repo => repo.GetEventParticipants(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(participants);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.UserId == 1);
            result.Should().Contain(p => p.UserId == 2);

            _participantRepositoryMock.Verify(repo => repo.GetEventParticipants(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_Event_Has_No_Participants()
        {
            // Arrange
            var query = new GetEventParticipantsQuery(2);

            _participantRepositoryMock.Setup(repo => repo.GetEventParticipants(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Participant>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _participantRepositoryMock.Verify(repo => repo.GetEventParticipants(2, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
