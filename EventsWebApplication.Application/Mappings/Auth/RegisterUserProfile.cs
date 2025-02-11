using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RegisterUser;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.Mappings.Auth
{
    public class RegisterUserProfile : Profile
    {

        public RegisterUserProfile()
        {
            CreateMap<RegisterUserCommand, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.BirthdayDate, opt => opt.MapFrom(src => src.Birthday));
        }
         
    }
}
