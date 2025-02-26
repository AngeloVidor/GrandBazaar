using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.DAL.Context;
using Cart.DAL.Interfaces;
using Cart.DAL.Interfaces.Management;
using Cart.Domain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.DAL.Repositories.Management
{
    public class CartManagementRepository : ICartManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICartRepository _cartRepository;

        public CartManagementRepository(ApplicationDbContext dbContext, ICartRepository cartRepository)
        {
            _dbContext = dbContext;
            _cartRepository = cartRepository;
        }

        public async Task<Item> AddItemIntoCartAsync(Item item)
        {
            var existingItem = await _dbContext.Items
                .FirstOrDefaultAsync(x => x.Cart_Id == item.Cart_Id && x.Product_Id == item.Product_Id);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                await _dbContext.Items.AddAsync(item);
            }

            await _dbContext.SaveChangesAsync();
            return item;
        }


        public async Task<Item> DeleteItemFromCartAsync(long cartId, long productId, int quantity)
        {
            Console.WriteLine($"Received CartId: {cartId}");
            Console.WriteLine($"Received ProductId: {productId}");

            var selectedProduct = await _dbContext.Items.FirstOrDefaultAsync(x => x.Cart_Id == cartId && x.Product_Id == productId);
            if (selectedProduct == null)
            {
                throw new KeyNotFoundException("Cart or item product not found");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            if (quantity > selectedProduct.Quantity)
            {
                throw new InvalidOperationException("Quantity to remove exceeds quantity in cart");
            }

            var amountToRemove = selectedProduct.Price * quantity;

            selectedProduct.Quantity -= quantity;

            if (selectedProduct.Quantity == 0)
            {
                _dbContext.Items.Remove(selectedProduct);
            }
            else
            {
                _dbContext.Items.Update(selectedProduct);
            }

            var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Cart_Id == cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found");
            }

            cart.TotalPrice -= amountToRemove;

            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
            return selectedProduct;
        }

        public async Task<IEnumerable<Item>> GetItemsFromCartAsync(long cartId)
        {
            var item = await _dbContext.Items.Where(x => x.Cart_Id == cartId).ToListAsync();
            if (item == null)
            {
                throw new KeyNotFoundException("Cart not found or is empty");
            }
            return item;
        }

    }
}