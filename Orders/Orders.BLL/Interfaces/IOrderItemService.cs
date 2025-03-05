using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.DTOs;

namespace Orders.BLL.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItemDto>> SaveOrderItemsAsync(List<OrderItemDto> orderItem);
    }
}