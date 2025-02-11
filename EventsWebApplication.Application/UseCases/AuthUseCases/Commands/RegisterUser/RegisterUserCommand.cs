using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string Name,
        string Surname,
        DateOnly Birthday,
        string Email,
        string Password,
        string ConfirmPassword
    ) : IRequest;
}
