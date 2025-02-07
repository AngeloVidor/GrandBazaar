using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;
using Auth.Domain.Entities;
using AutoMapper;

namespace Auth.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegistrationDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
        }
    }
}