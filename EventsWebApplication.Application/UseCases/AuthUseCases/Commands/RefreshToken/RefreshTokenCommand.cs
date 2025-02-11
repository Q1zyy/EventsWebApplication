using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token) : IRequest<string>;
}
