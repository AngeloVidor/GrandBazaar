using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces;
using Cart.Domain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShoppingCart> AddNewCartAsync(ShoppingCart cart)
        {
            await _dbContext.Carts.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<ShoppingCart> UpdateTotalPriceAsync(decimal totalPrice, long cartId)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Cart_Id == cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found");
            }
            cart.TotalPrice = totalPrice;

            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }
    }
}