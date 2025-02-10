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

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Participant?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Participant>> GetEventParticipants(int id, CancellationToken cancellationToken)
        {
             return await _context.Participants.Where(p => p.EventId == id)
                .Include(p => p.User) 
                .ToListAsync();
        }

        public async Task<Participant?> GetParticipantByEventAndUserId(int eventId, int userId, CancellationToken cancellationToken)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId, cancellationToken);
        }
    }
}
