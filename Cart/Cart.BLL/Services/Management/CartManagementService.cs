using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces.Management;
using Cart.DAL.Interfaces.Management;

namespace Cart.BLL.Services.Management
{
    public class CartManagementService : ICartManagementService
    {
        private readonly ICartManagementRepository _cartManagementRepository;
        private readonly IMapper _mapper;

        public CartManagementService(ICartManagementRepository cartManagementRepository, IMapper mapper)
        {
            _cartManagementRepository = cartManagementRepository;
            _mapper = mapper;
        }

        public async Task<ItemDto> AddItemIntoCartAsync(ItemDto item)
        {
            throw new NotImplementedException();

            // item.Cart_Id
            // item.Buyer_Id
        }
    }
}