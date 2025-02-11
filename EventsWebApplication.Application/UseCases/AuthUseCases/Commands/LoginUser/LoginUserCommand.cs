using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResult>;
}
