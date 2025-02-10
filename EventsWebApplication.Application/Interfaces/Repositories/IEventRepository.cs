using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        public Task<PaginatedList<Event>> GetAllEvents(int pageNo = 1, int pageSize = 2, CancellationToken cancellationToken);

    }
}
