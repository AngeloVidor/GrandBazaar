using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.DAL.Context;
using Products.DAL.Interfaces;
using Products.DAL.Interfaces.Provider;
using Products.Domain.Entities;

namespace Products.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductProviderRepository _productProviderRepository;

        public ProductRepository(ApplicationDbContext dbContext, IProductProviderRepository productProviderRepository)
        {
            _dbContext = dbContext;
            _productProviderRepository = productProviderRepository;
        }

        public async Task<Product> AddNewProductAsync(Product product)
        {
            product.Category.ToString();
            product.Quality.ToString();

            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var selectedProduct = await _productProviderRepository.GetProductByIdAsync(product.Product_Id);

            selectedProduct.ProductName = product.ProductName;
            selectedProduct.Product_Id = product.Product_Id;
            selectedProduct.Description = product.Description;
            selectedProduct.Price = product.Price;
            selectedProduct.StockQuantity = product.StockQuantity;
            selectedProduct.Seller_Id = product.Seller_Id;
            selectedProduct.Category = product.Category;
            selectedProduct.DateAdded = product.DateAdded;
            selectedProduct.Quality = product.Quality;

            selectedProduct.Category.ToString();
            selectedProduct.Quality.ToString();
            
            _dbContext.Products.Update(selectedProduct);
            await _dbContext.SaveChangesAsync();
            return selectedProduct;
        }
    }
}