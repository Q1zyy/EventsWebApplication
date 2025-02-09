using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.UsersUseCases.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {

        private readonly IUserRepository _userRepository;

        public GetUserByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (result == null)
            {
                throw new Exception($"No user with email {request.Email}");
            }
            return result;
        }
    }
}
