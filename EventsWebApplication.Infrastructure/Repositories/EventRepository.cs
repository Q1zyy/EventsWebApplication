using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
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

        public async Task<PaginatedList<Event>> GetAllEvents(int pageNo = 1, int pageSize = 2, CancellationToken cancellationToken = default)
        {
            int pages = (_context.Events.Count() + pageSize - 1) / pageSize;
            int skip = pageSize * (pageNo - 1);
            var result = await _context.Events.Skip(skip).Take(pageSize).ToListAsync();
            return new PaginatedList<Event>
            {
                Items = result,
                CurrentPage = pageNo,
                TotalPages = pages
            };
        }

        public async override Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

    }
}
