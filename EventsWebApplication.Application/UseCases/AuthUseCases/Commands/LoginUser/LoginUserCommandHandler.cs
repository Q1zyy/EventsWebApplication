using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Auth;
using EventsWebApplication.Application.Interfaces.Repositories;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {

        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserCommandHandler(
            IJwtService jwtService,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                throw new BadRequestException("No such user.");
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new BadRequestException("Incorrect credentials.");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new LoginUserResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = user
            };

        }
    }
}
