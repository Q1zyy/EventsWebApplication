using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Data
{
    public static class DatabaseSeeder
    {

        public static void Seed(AppDbContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.Migrate();


            var categories = new List<Category>
            {
                new Category { Title = "Concert", Description = "Just a simple description" },
                new Category { Title = "Sport", Description = "Sport events" },
                new Category { Title = "Theatre", Description = "Alalalalalalaalalalal" }
            };

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(categories);
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(new List<Event>
                {
                    new Event {
                        Title = "Rock concert",
                        Category = categories[0],
                        Description = "Just a concert",
                        EventDateTime = DateTime.Now.AddDays(10),
                        ParticipantsMaxCount = 20,
                        Place = "BSUIR",
                        Images = null,
                        Participants = new List<Participant>()
                    }, 
                    new Event {
                        Title = "Squid game",
                        Category = categories[1],
                        Description = "Kukuma ko ti tire kuku ma",
                        EventDateTime = DateTime.Now.AddDays(2),
                        ParticipantsMaxCount = 5,
                        Place = "Japan",
                        Images = null,
                        Participants = new List<Participant>()
                    }
                });
            }

            var passwordHasher = new PasswordHasher();

            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>
                {
                    new User {
                        Name = "Evgeniy",
                        Surname = "Gudoryan",
                        BirthdayDate = new DateOnly(2005, 6, 7),
                        Email = "gudoryanjackson@mail.ru",
                        Role = Domain.Enums.UserRole.Admin,
                        PasswordHash = passwordHasher.Hash("123456789"),
                        Events = new List<Event>()
                    },
                    new User {
                        Name = "Kirill",
                        Surname = "Detkovsky",
                        BirthdayDate = new DateOnly(2005, 2, 27),
                        Email = "paskyshka@mail.ru",
                        Role = Domain.Enums.UserRole.User,
                        PasswordHash = passwordHasher.Hash("paskishka"),
                        Events = new List<Event>()
                    }

                });
            }

            context.SaveChanges();
        }
    }
}
