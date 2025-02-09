﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using MediatR;

namespace EventsWebApplication.Application.UseCases.CategoryUseCases.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Category>;
}
