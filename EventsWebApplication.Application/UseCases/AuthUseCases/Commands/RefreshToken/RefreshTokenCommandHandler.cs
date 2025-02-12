using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Auth;
using EventsWebApplication.Application.Interfaces.Repositories;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(request.Token, cancellationToken);
            
            if (user == null)
            {
                throw new NotFoundException("No such user");
            }
            
            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Token expired");
            }

            var newToken = _jwtService.GenerateAccessToken(user);
            return newToken;

        }
    }
}
