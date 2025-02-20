using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces;
using Cart.BLL.Interfaces.Management;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartManagementController : ControllerBase
    {
        private readonly ICartManagementService _cartService;

        public CartManagementController(ICartManagementService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemIntoCart([FromBody] ItemDto item)
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
    }
}