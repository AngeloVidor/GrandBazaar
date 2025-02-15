using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.BLL.DTOs;
using Products.BLL.Interfaces;
using Products.BLL.Messaging.Events.Interfaces;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ITransferUserToSellerEvent _transfer;


        public ProductController(IProductService productService, ITransferUserToSellerEvent transfer)
        {
            _productService = productService;
            _transfer = transfer;
        }

        [HttpPost("product")]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdString = HttpContext.Items["userId"]?.ToString();
                if (userIdString == null)
                {
                    return StatusCode(401, "User must be logged in.");
                }

                long userId = long.Parse(userIdString);

                product.Seller_Id = await _transfer.GetSellerIdAsync(userId);

                var newProduct = await _productService.AddNewProductAsync(product);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("product")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto product)
        {
            try
            {
                var userIdString = HttpContext.Items["userId"]?.ToString();
                if (userIdString == null)
                {
                    return StatusCode(401, "User must be logged in.");
                }
                long userId = long.Parse(userIdString);
                product.Seller_Id = await _transfer.GetSellerIdAsync(userId);

                var updatedProduct = await _productService.UpdateProductAsync(product);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProduct(long productId)
        {
            try
            {
                var deletedProduct = await _productService.RemoveProductAsync(productId);
                return Ok(deletedProduct);
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