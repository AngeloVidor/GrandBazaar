using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces.Management;
using Cart.Domain.Domain.Entities;

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
    }
}