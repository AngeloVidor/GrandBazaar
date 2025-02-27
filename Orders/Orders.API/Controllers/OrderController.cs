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
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                //order.Costumer_Id: Enviar o UserId para o Buyer, receber o Buyer_Id e adicionar ao Costumer_Id;
                var userId = HttpContext.Items["userId"].ToString();
                Console.WriteLine($"Received UserId: {userId}");
                return Ok("Hello, world!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}