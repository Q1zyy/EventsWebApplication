using System.Reflection;
using EventsWebApplication.Application.Interfaces.Auth;
using EventsWebApplication.Application.Interfaces.Image;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Application.Mappings.Events;
using EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoriesAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


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
builder.Services.AddScoped<IImageService, ImageService>();


builder.Services.AddAutoMapper(typeof(CreateEventProfile));
builder.Services.AddAutoMapper(typeof(UpdateEventProfile));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetEventByIdQuery).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



var app = builder.Build();

app.UseCors("AllowAll");

app.UseStaticFiles();

var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

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
