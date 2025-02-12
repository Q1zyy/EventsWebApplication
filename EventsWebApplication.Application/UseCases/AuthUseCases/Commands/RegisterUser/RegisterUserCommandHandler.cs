using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.Interfaces.Auth;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var have = await _userRepository.IsUserExistByEmail(request.Email, cancellationToken);
            if (have)
            {
                throw new AlreadyExistsException("User with this email already exists");
            }
            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.Hash(request.Password);
            user.Role = Domain.Enums.UserRole.User;
            await _userRepository.AddAsync(user, cancellationToken);
        }
    }
}
