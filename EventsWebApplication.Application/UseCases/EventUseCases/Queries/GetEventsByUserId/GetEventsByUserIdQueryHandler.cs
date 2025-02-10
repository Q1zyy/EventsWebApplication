using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByUserId
{
    public class GetEventsByUserIdQueryHandler : IRequestHandler<GetEventsByUserIdQuery, IEnumerable<Event>>
    {

        private readonly IUserRepository _userRepository;

        public GetEventsByUserIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Event>> Handle(GetEventsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetEventsByUserId(request.Id, cancellationToken);
        }
    }
}
