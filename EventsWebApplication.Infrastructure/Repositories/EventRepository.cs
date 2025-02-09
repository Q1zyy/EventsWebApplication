using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
 
        }

        public async override Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetEventParticipants(int id, CancellationToken cancellationToken)
        {
            return _context.Events
                .Where(e => e.Id == id)
                .SelectMany(e => e.Participants.Select(p => p.User))
                .Where(u => u != null);
        }
    }
}
