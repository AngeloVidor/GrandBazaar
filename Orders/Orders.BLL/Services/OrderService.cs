using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.DTOs;
using Orders.BLL.Interfaces;
using Orders.BLL.Messaging.Costumer.Interfaces;
using Orders.BLL.Messaging.Products.Interfaces;
using Orders.DAL.Interfaces;
using Orders.Domain.Entities;

namespace Orders.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationPub _publisher;
        private readonly IProductsRequestPublisher _productsRequest;


        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserIdentificationPub publisher, IProductsRequestPublisher productsRequest)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _publisher = publisher;
            _productsRequest = productsRequest;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto order, long userId)
        {
            //var orderEntity = _mapper.Map<Order>(order);
            var buyerId = await _publisher.GetCostumerIdAsync(userId);
            Console.WriteLine("sent");
            order.Costumer_Id = buyerId;

            Console.WriteLine("sending...");
            var response = await _productsRequest.Publish(buyerId);
            if (response != null)
            {
                foreach (var product in response.Products)
                {
                    System.Console.WriteLine($"Product: {product.ToString()}");
                }
            }
            
            //var addedOrder = await _orderRepository.CreateOrderAsync(orderEntity);
            //return _mapper.Map<OrderDto>(addedOrder);
            return order;
        }
    }
}