using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sellers.BLL.Interfaces.Filters;

namespace Sellers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellersFiltersController : ControllerBase
    {
        private readonly ISellersFiltersService _sellersFiltersService;

        public SellersFiltersController(ISellersFiltersService sellersFiltersService)
        {
            _sellersFiltersService = sellersFiltersService;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetSellersByCategory(int category)
        {
            try
            {
                var products = await _sellersFiltersService.GetSellersByCategoryAsync(category);
                return Ok(products);
            }
            catch (KeyNotFoundException ex)
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