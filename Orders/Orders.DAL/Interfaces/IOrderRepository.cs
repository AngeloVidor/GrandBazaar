using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Entities;

namespace Orders.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetMyOrdersAsync(long costumerId);
        Task<Order> GetOrderByIdAsync(long orderId);
    }
}