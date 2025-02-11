using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<bool> IsUserExistByEmail(string email, CancellationToken cancellationToken);

        Task<IEnumerable<Event>> GetEventsByUserId(int id, CancellationToken cancellationToken);

        Task<User> GetUserByRefreshToken(string refresh, CancellationToken cancellationToken);

    }
}
