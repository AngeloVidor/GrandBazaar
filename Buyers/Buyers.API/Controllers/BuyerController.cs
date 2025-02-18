using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.DTOs;
using Buyers.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buyers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpPost("buyer")]
        public async Task<IActionResult> AddBuyer([FromBody] BuyerDto buyer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                buyer.User_Id = long.Parse(HttpContext.Items["userId"].ToString());
                var addedBuyer = await _buyerService.AddNewBuyerAsync(buyer);
                return Ok(addedBuyer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
                var userId = long.Parse(HttpContext.Items["userId"].ToString());
                var buyer = await _buyerService.GetMyProfileAsync(userId);
                return Ok(buyer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}