using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces;
using Cart.BLL.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Interfaces.ProductHandler;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductHandlerService _productHandlerServ;

        public CartController(ICartService cartService, IProductHandlerService productHandlerServ)
        {
            _cartService = cartService;
            _productHandlerServ = productHandlerServ;
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddNewCart([FromBody] ShoppingCartDto cart)
        {
            try
            {
                long userId = long.Parse(HttpContext.Items["userId"].ToString());
                if (userId == null || userId <= 0)
                {
                    return StatusCode(400, "User not found");
                }
                var newCart = await _cartService.AddNewCartAsync(cart, userId);
                return Ok(newCart);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //just testing...
        [HttpPost]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productHandlerServ.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }
    }
}