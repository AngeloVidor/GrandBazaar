using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces;
using Cart.BLL.Messaging.Events.Interfaces;
using Cart.DAL.Interfaces;
using Cart.Domain.Domain.Entities;

namespace Cart.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ITransferBuyerToCartEvent _transferBuyerToCartEvent;

        public CartService(ICartRepository cartRepository, IMapper mapper, ITransferBuyerToCartEvent transferBuyerToCartEvent)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _transferBuyerToCartEvent = transferBuyerToCartEvent;
        }

        public async Task<ShoppingCartDto> AddNewCartAsync(ShoppingCartDto cartDto, long userId)
        {
            var cartEntity = _mapper.Map<ShoppingCart>(cartDto);

            cartDto.Buyer_Id = await _transferBuyerToCartEvent.GetBuyerIdAsync(userId);
            if(cartDto.Buyer_Id <= 0)
            {
                throw new InvalidOperationException("Buyer not found");
            }

            var response = await _cartRepository.AddNewCartAsync(cartEntity);
            return _mapper.Map<ShoppingCartDto>(response);
        }
    }
}