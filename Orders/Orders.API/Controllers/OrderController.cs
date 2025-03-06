using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.BLL.DTOs;
using Orders.BLL.Interfaces;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder(OrderDto order)
        {

            long userId = long.Parse(HttpContext.Items["userId"].ToString());

            try
            {
                var addedOrder = await _orderService.CreateOrderAsync(order, userId);
                return Ok(addedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("orders")]
        public async Task<IActionResult> MyOrders()
        {
            try
            {
                var userId = long.Parse(HttpContext.Items["userId"].ToString());

                var orders = await _orderService.GetMyOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}