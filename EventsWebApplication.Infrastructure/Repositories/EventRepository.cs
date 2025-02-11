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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
 
        }

        public async Task<PaginatedList<Event>> GetAllEvents(CancellationToken cancellationToken = default, int pageNo = 1, int pageSize = 2)
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

        public async Task<PaginatedList<Event>> GetEventsWithFilters(CancellationToken cancellationToken, string? title, DateTime? eventFrom, DateTime? eventTo, string? place, int? categoryId, int pageNo = 1, int pageSize = 2)
        {
            IQueryable<Event> query = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(e => e.Title.Contains(title));
            }

            if (eventFrom.HasValue)
            {
                query = query.Where(e => e.EventDateTime >= eventFrom.Value);
            }

            if (eventTo.HasValue)
            {
                query = query.Where(e => e.EventDateTime <= eventTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(place))
            {
                query = query.Where(e => e.Place!.Contains(place));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(e => e.Category != null && e.Category.Id == categoryId);
            }

            int totalCount = await query.CountAsync() / pageSize;

            List<Event> events = await query
                .OrderBy(e => e.EventDateTime) 
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Event>
            {
                Items = events,
                CurrentPage = pageNo,
                TotalPages = totalCount
            };
        }
    }
}
