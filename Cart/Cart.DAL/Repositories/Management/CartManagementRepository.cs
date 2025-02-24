using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces.Management;
using Cart.Domain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.DAL.Repositories.Management
{
    public class CartManagementRepository : ICartManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartManagementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Item> AddItemIntoCartAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Item> DeleteItemFromCartAsync(long itemId, long cartId)
        {
            var item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Cart_Id == cartId && x.Item_Id == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException("Cart or item not found");
            }
            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsFromCartAsync(long cartId)
        {
            var item = await _dbContext.Items.Where(x => x.Cart_Id == cartId).ToListAsync();
            if (item == null)
            {
                throw new KeyNotFoundException("Cart not found or is empty");
            }
            return item;
        }

    }
}