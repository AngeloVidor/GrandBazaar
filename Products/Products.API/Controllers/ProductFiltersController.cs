using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.BLL.Interfaces.Filters;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductFiltersController : ControllerBase
    {
        private readonly IProductFilterService _productFilterService;

        public ProductFiltersController(IProductFilterService productFilterService)
        {
            _productFilterService = productFilterService;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetProductsByCategory(int category)
        {
            try
            {
                var productsCategory = await _productFilterService.GetProductsByCategoryAsync(category);
                return Ok(productsCategory);
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

        [HttpGet("quality")]
        public async Task<IActionResult> GetProductsByQuality(int quality)
        {
            try
            {
                var productsQuality = await _productFilterService.GetProductByQualityAsync(quality);
                return Ok(productsQuality);
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