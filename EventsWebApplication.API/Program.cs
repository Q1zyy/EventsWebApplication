using System.Reflection;
using EventsWebApplication.Application.Interfaces.Auth;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoriesAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("EventsWebApplication.Infrastructure")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();


builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetEventByIdQuery).Assembly);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DatabaseSeeder.Seed(dbContext);
}

app.Run();
