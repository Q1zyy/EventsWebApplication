using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Tests.RepositoryTests
{
    public class GetByIdAsyncTests
    {

        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new EventRepository(context);

            var testEvent = new Event
            {
                Id = 552,
                Title = "Test Event",
                EventDateTime = DateTime.UtcNow,
                Category = new Category { Id = 12, Title = "Music" },
                Place = "asdads",
                ParticipantsMaxCount = 10,
                Participants = new List<Participant> { new Participant { Id = 1 } }
            };

            context.Events.Add(testEvent);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(552, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Event");
            result.Category.Should().NotBeNull();
            result.Participants.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new EventRepository(context);

            // Act
            var result = await repository.GetByIdAsync(999, CancellationToken.None); 

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEventWithCategoryAndParticipants()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new EventRepository(context);

            var testEvent = new Event
            {
                Id = 442,
                Title = "Another Event",
                EventDateTime = DateTime.UtcNow,
                Category = new Category { Id = 32, Title = "Tech"},
                Place = "asdads",
                ParticipantsMaxCount = 10,
                Participants = new List<Participant>
                {
                    new Participant { Id = 2 },
                    new Participant { Id = 3 }
                }
            };

            context.Events.Add(testEvent);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(442, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Another Event");
            result.Category.Should().NotBeNull();
            result.Category!.Title.Should().Be("Tech");
            result.Participants.Should().HaveCount(2);
        }

    }
}
