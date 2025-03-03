using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Entities;

namespace Orders.DAL.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> SaveOrderItemsAsync(OrderItem orderItem);
    }
}