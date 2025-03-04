using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces.Handler;
using Cart.DAL.Interfaces.Handler;

namespace Cart.BLL.Services.Handler
{
    public class CartHandlerService : ICartHandlerService
    {
        private readonly ICartHandlerRepository _cartHandlerRepository;
        private readonly IMapper _mapper;

        public CartHandlerService(ICartHandlerRepository cartHandlerRepository, IMapper mapper)
        {
            _cartHandlerRepository = cartHandlerRepository;
            _mapper = mapper;
        }

        public async Task<long> GetBuyerIdByCartIdAsync(long cartId)
        {
            if (cartId <= 0)
            {
                throw new ArgumentException("Invalid Cart_Id");
            }
            return await _cartHandlerRepository.GetBuyerIdByCartIdAsync(cartId);
        }

        public async Task<long> GetCartIdByCostumerIdAsync(long userId)
        {
            return await _cartHandlerRepository.GetCartIdByCostumerIdAsync(userId);
        }

        public async Task<long> GetCartIdByUserIdAsync(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User_Id");
            }
            return await _cartHandlerRepository.GetCartIdByUserIdAsync(userId);
        }

        public async Task<List<ItemDto>> GetItemsFromCartByCartIdAsync(long cartId)
        {
            var items = await _cartHandlerRepository.GetItemsFromCartByCartIdAsync(cartId);
            return _mapper.Map<List<ItemDto>>(items);
        }
    }
}