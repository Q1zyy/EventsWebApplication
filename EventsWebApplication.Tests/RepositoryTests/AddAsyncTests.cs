using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Tests.RepositoryTests
{
    public class AddAsyncTests
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
        public async Task AddAsync_ShouldSaveEventToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new EventRepository(context);
            var category = new Category
            {
                Id = 1,
                Title = "123"
            };
            var newEvent = new Event
            {
                Id = 2,
                Title = "New Event",
                Place = "adasdas",
                ParticipantsMaxCount = 10,
                Category = category,
                EventDateTime = DateTime.UtcNow
            };

            // Act
            await repository.AddAsync(newEvent, CancellationToken.None);
            var savedEvent = await context.Events.FindAsync(2);

            // Assert
            savedEvent.Should().NotBeNull();
            savedEvent!.Title.Should().Be("New Event");
        }


        [Fact]
        public async Task AddAsync_ShouldSaveCategoryToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category
            {
                Id = 2,
                Title = "123"
            };
           

            // Act
            await repository.AddAsync(category, CancellationToken.None);
            var savedCategory = await context.Categories.FindAsync(2);

            // Assert
            savedCategory.Should().NotBeNull();
            savedCategory!.Title.Should().Be("123");
        }
    }
}