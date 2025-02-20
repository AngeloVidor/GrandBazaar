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
        private readonly IBuyerIdentificationPublisher _publisher;

        public CartService(ICartRepository cartRepository, IMapper mapper, IBuyerIdentificationPublisher publisher)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<ShoppingCartDto> AddNewCartAsync(ShoppingCartDto cartDto, long userId)
        {
            var cartEntity = _mapper.Map<ShoppingCart>(cartDto);

            cartEntity.Buyer_Id = await _publisher.GetBuyerIdAsync(userId);
            Console.WriteLine($"BuyerId: {cartEntity.Buyer_Id}");

            var response = await _cartRepository.AddNewCartAsync(cartEntity);
            return _mapper.Map<ShoppingCartDto>(response);
        }
    }
}