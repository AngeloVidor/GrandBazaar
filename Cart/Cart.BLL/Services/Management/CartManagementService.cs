using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.Management;
using Cart.BLL.Messaging.Events.Interfaces.ProductValidator;
using Cart.DAL.Interfaces.Management;
using Cart.Domain.Domain.Entities;

namespace Cart.BLL.Services.Management
{
    public class CartManagementService : ICartManagementService
    {
        private readonly ICartManagementRepository _cartManagementRepository;
        private readonly ICartHandlerService _cartHandlerService;
        private readonly IMapper _mapper;
        private readonly IProductValidatorPublisher _publisher;

        public CartManagementService(ICartManagementRepository cartManagementRepository, IMapper mapper, ICartHandlerService cartHandlerService, IProductValidatorPublisher publisher)
        {
            _cartManagementRepository = cartManagementRepository;
            _mapper = mapper;
            _cartHandlerService = cartHandlerService;
            _publisher = publisher;
        }

        public async Task<ItemDto> AddItemIntoCartAsync(ItemDto item, long userId)
        {
            var itemEntity = _mapper.Map<Item>(item);

            var response = await _publisher.Publish(itemEntity.Quantity, itemEntity.Product_Id);
            if (response.ErrorMessage != null)
            {
                throw new Exception($"Failed to add item to cart: {response.ErrorMessage}");
            }

            itemEntity.TotalPrice += response.Price;
            itemEntity.Cart_Id = await _cartHandlerService.GetCartIdByUserIdAsync(userId);
            itemEntity.Buyer_Id = await _cartHandlerService.GetBuyerIdByCartIdAsync(itemEntity.Cart_Id);

            var addedItem = await _cartManagementRepository.AddItemIntoCartAsync(itemEntity);
            return _mapper.Map<ItemDto>(addedItem);
        }
    }
}