using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    }
}
