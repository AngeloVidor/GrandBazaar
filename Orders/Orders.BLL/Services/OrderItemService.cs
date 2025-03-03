using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.DTOs;
using Orders.BLL.Interfaces;
using Orders.DAL.Interfaces;
using Orders.Domain.Entities;

namespace Orders.BLL.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemDto> SaveOrderItemsAsync(OrderItemDto orderItem)
        {
            var itemEntity = _mapper.Map<OrderItem>(orderItem);
            var result = await _orderItemRepository.SaveOrderItemsAsync(itemEntity);
            return _mapper.Map<OrderItemDto>(result);
        }

    }
}