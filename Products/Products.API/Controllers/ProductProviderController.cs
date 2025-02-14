using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.BLL.Interfaces;
using Products.BLL.Interfaces.Provider;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductProviderController : ControllerBase
    {
        private readonly IProductProviderService _productProviderService;

        public ProductProviderController(IProductProviderService productProviderService)
        {
            _productProviderService = productProviderService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productProviderService.GetAllProductsForDisplayAsync();
                return Ok(products);
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