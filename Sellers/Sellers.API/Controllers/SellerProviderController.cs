using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sellers.BLL.Interfaces.Provider;

namespace Sellers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerProviderController : ControllerBase
    {
        private readonly ISellerProviderService _sellerProviderService;

        public SellerProviderController(ISellerProviderService sellerProviderService)
        {
            _sellerProviderService = sellerProviderService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSellerById(long sellerId)
        {
            try
            {
                var seller = await _sellerProviderService.GetSellerByIdAsync(sellerId);
                return Ok(seller);
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
    }
}