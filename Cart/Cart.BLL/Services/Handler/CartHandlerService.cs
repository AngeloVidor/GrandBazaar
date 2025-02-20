using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Interfaces.Handler;
using Cart.DAL.Interfaces.Handler;

namespace Cart.BLL.Services.Handler
{
    public class CartHandlerService : ICartHandlerService
    {
        private readonly ICartHandlerRepository _cartHandlerRepository;

        public CartHandlerService(ICartHandlerRepository cartHandlerRepository)
        {
            _cartHandlerRepository = cartHandlerRepository;
        }

        public async Task<long> GetBuyerIdByCartIdAsync(long cartId)
        {
            if (cartId <= 0)
            {
                throw new ArgumentException("Invalid Cart_Id");
            }
            return await _cartHandlerRepository.GetBuyerIdByCartIdAsync(cartId);
        }

        public async Task<long> GetCartIdByUserIdAsync(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User_Id");
            }
            return await _cartHandlerRepository.GetCartIdByUserIdAsync(userId);
        }
    }
}