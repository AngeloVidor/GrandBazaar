using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.DTOs;

namespace Orders.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderDto order, long userId);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(long costumerId);
    }
}