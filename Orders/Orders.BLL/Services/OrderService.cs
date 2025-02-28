using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.DTOs;
using Orders.BLL.Interfaces;
using Orders.BLL.Messaging.Costumer.Interfaces;
using Orders.DAL.Interfaces;
using Orders.Domain.Entities;

namespace Orders.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationPub _publisher;


        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserIdentificationPub publisher)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto order, long userId)
        {
            //var orderEntity = _mapper.Map<Order>(order);
            var buyerId = await _publisher.GetCostumerIdAsync(userId);
            order.Costumer_Id = buyerId;
            Console.WriteLine($"BuyerID: {buyerId}");

            //var addedOrder = await _orderRepository.CreateOrderAsync(orderEntity);
            //return _mapper.Map<OrderDto>(addedOrder);
            return order;
        }
    }
}