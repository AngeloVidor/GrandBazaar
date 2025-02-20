using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces.Handler;
using Microsoft.EntityFrameworkCore;

namespace Cart.DAL.Repositories.Handler
{
    public class CartHandlerRepository : ICartHandlerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartHandlerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> GetBuyerIdByCartIdAsync(long cartId)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Cart_Id == cartId);
            return cart.Buyer_Id;
        }

        public async Task<long> GetCartIdByUserIdAsync(long userId)
        {
            var userCart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.User_Id == userId);
            return userCart.Cart_Id;
        }

        public async Task<bool> CartAlreadyExistsAsync(long userId)
        {
            var cart = _dbContext.Carts.FirstOrDefaultAsync(x => x.User_Id == userId);
            if (cart != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}