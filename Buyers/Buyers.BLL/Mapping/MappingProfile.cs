using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Buyers.BLL.DTOs;
using Buyers.Domain.Domain;

namespace Buyers.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Buyer, BuyerDto>().ReverseMap();
        }
    }
}