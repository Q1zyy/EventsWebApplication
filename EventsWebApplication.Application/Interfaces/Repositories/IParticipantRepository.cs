using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Interfaces.Repositories
{
    public interface IParticipantRepository : IRepository<Participant>
    {

        public Task<IEnumerable<Participant>> GetEventParticipants(int id, CancellationToken cancellationToken);

        public Task<Participant?> GetParticipantByEventAndUserId(int eventId, int userId, CancellationToken cancellationToken);

    }
}
