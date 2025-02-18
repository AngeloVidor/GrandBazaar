using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces;
using Cart.Domain.Domain.Entities;

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
    }
}