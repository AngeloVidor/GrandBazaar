using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.DAL.Context;
using Orders.DAL.Interfaces;
using Orders.Domain.Entities;

namespace Orders.DAL.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderItem>> SaveOrderItemsAsync(List<OrderItem> orderItem)
        {
            await _dbContext.Items.AddRangeAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return orderItem;
        }
    }
}