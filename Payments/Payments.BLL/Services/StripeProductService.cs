using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Payments.BLL.Interfaces;
using Payments.Domain.Entities;
using Stripe;

namespace Payments.BLL.Services
{
    public class StripeProductService : IStripeProductService
    {
        private readonly string _secretKey;
        private readonly ProductService _productService;
        private readonly PriceService _priceService;

        public StripeProductService(IConfiguration configuration, ProductService productService, PriceService priceService)
        {
            _secretKey = configuration["STRIPE_SECRET_KEY"];
            StripeConfiguration.ApiKey = _secretKey;
            _productService = productService;
            _priceService = priceService;
        }
        public async Task<bool> CreateProductAsync(AppProduct product)
        {
            try
            {
                var options = new ProductCreateOptions
                {
                    Name = product.ProductName,
                    Description = product.Description
                };

                var stripeProduct = await _productService.CreateAsync(options);
                if (stripeProduct == null || string.IsNullOrEmpty(stripeProduct.Id))
                {
                    return false;
                }

                var price = new PriceCreateOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)product.UnitAmount,
                    Product = stripeProduct.Id
                };
                var createdProduct = await _priceService.CreateAsync(price);
                if (createdProduct == null || string.IsNullOrEmpty(createdProduct.Id))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating Stripe Product " + ex.Message);
            }


        }
    }
}