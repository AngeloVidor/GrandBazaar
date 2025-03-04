using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.Domain.Domain.Entities;

namespace Cart.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();
            CreateMap<Item, CreatorItemDto>().ReverseMap();
            CreateMap<Item, ItemDto>().ReverseMap();


        }
    }
}