using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeletePhotoFromEvent
{
    public record DeletePhotoFromEventCommand(int Id, string Image) : IRequest;
}
