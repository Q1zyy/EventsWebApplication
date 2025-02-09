using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {

        Task<IEnumerable<User>> GetEventParticipants(int id, CancellationToken cancellationToken);

    }
}
