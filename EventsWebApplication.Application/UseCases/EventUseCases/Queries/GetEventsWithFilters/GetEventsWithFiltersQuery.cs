using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithFilters
{
    public record GetEventsWithFiltersQuery(
        string? Title,
        DateTime? DateFrom,
        DateTime? DateTo,
        string? Place,
        int? CategoryId,
        int pageNo = 1,
        int pageSize = 2
    ) : IRequest<PaginatedList<Event>>;
}
