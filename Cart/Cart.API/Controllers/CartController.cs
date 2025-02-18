using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddNewCart([FromBody] ShoppingCartDto cart)
        {
            try
            {
                long userId = long.Parse(HttpContext.Items["userId"].ToString());
                var newCart = await _cartService.AddNewCartAsync(cart, userId);
                return Ok(newCart);
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}