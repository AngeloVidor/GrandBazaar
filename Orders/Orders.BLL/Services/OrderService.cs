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
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationPub _publisher;
        private readonly IProductsRequestPublisher _productsRequest;


        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserIdentificationPub publisher, IProductsRequestPublisher productsRequest, IOrderItemService orderItemService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _publisher = publisher;
            _productsRequest = productsRequest;
            _orderItemService = orderItemService;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto order, long userId)
        {
            var orderEntity = _mapper.Map<Order>(order);
            
            var buyerId = await _publisher.GetCostumerIdAsync(userId);

            orderEntity.Costumer_Id = buyerId;
            orderEntity.PaymentStatus = PaymentStatus.Pending;
            orderEntity.Status = OrderStatus.PendingPayment;

            var response = await _productsRequest.Publish(buyerId);
            if (response != null && response.Products.Any())
            {
                orderEntity.Products = response.Products.Select(product => new OrderItem
                {
                    Product_Id = product.Product_Id,
                    ProductName = product.ProductName,
                    Quantity = product.Quantity,
                    Price = product.Price,

                }).ToList();

                orderEntity.TotalAmount = response.Products.Select(p => p.Price * p.Quantity).Sum();
            }
            var addedOrder = await _orderRepository.CreateOrderAsync(orderEntity);

            await _orderItemService.SaveOrderItemsAsync(_mapper.Map<List<OrderItemDto>>(orderEntity.Products));
            return _mapper.Map<OrderDto>(addedOrder);
        }
    }
}