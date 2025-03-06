using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.DAL.Context;
using Orders.DAL.Interfaces;
using Orders.Domain.Entities;

namespace Orders.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetMyOrdersAsync(long costumerId)
        {
            return await _dbContext.Orders.Where(c => c.Costumer_Id == costumerId)
                        .Include(o => o.Products).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(long orderId)
        {
            return await _dbContext.Orders.Include(p => p.Products)
                .FirstOrDefaultAsync(o => o.Order_Id == orderId);
        }
    }
}