using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Mappings.Events
{
    public class CreateEventProfile : Profile
    {
        public CreateEventProfile()
        {
            CreateMap<CreateEventCommand, Event>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ParticipantsMaxCount, opt => opt.MapFrom(src => src.ParticipantsMaxCount))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.Place))
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime));
        }

    }
}
