using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.Management;
using Cart.DAL.Interfaces.Management;
using Cart.Domain.Domain.Entities;

namespace Cart.BLL.Services.Management
{
    public class CartManagementService : ICartManagementService
    {
        private readonly ICartManagementRepository _cartManagementRepository;
        private readonly ICartHandlerService _cartHandlerService;
        private readonly IMapper _mapper;

        public CartManagementService(ICartManagementRepository cartManagementRepository, IMapper mapper, ICartHandlerService cartHandlerService)
        {
            _cartManagementRepository = cartManagementRepository;
            _mapper = mapper;
            _cartHandlerService = cartHandlerService;
        }

        public async Task<ItemDto> AddItemIntoCartAsync(ItemDto item, long userId)
        {
            var itemEntity = _mapper.Map<Item>(item);
            itemEntity.Cart_Id = await _cartHandlerService.GetCartIdByUserIdAsync(userId);
            itemEntity.Buyer_Id = await _cartHandlerService.GetBuyerIdByCartIdAsync(itemEntity.Cart_Id);
            var addedItem = await _cartManagementRepository.AddItemIntoCartAsync(itemEntity);
            return _mapper.Map<ItemDto>(addedItem);
        }
    }
}