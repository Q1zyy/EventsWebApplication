using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsAll
{
    public record GetEventsAllQuery(int PageNo = 1, int PageSize = 2) : IRequest<PaginatedList<Event>>;
}
