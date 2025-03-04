using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.Management;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartManagementController : ControllerBase
    {
        private readonly ICartManagementService _cartService;
        private readonly ICartHandlerService _cartHandlerService;


        public CartManagementController(ICartManagementService cartService, ICartHandlerService cartHandlerService)
        {
            _cartService = cartService;
            _cartHandlerService = cartHandlerService;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemIntoCart([FromBody] CreatorItemDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = long.Parse(HttpContext.Items["userId"].ToString());
                if (userId <= 0)
                {
                    return BadRequest("Invalid User_Id");
                }
                var addedItem = await _cartService.AddItemIntoCartAsync(item, userId);
                return Ok(addedItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("item")]
        public async Task<IActionResult> DeleteItemFromCart(long productId, int quantity)
        {
            try
            {
                var userId = long.Parse(HttpContext.Items["userId"].ToString());
                if (userId <= 0)
                {
                    Console.WriteLine("Invalid userId");
                }
                Console.WriteLine($"UserId: {userId}");


                var cartId = await _cartHandlerService.GetCartIdByUserIdAsync(userId);
                if (cartId <= 0)
                {
                    Console.WriteLine("Invalid cartId");
                }
                System.Console.WriteLine($"CartId: {cartId}");
                var deletedItem = await _cartService.DeleteItemFromCartAsync(cartId, productId, quantity);
                if (deletedItem == null)
                {
                    Console.WriteLine("Not deleted");
                }
                return Ok(deletedItem);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
    }
}