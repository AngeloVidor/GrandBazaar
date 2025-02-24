using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Domain.Entities;

namespace Cart.DAL.Interfaces
{
    public interface ICartRepository
    {
        Task<ShoppingCart> AddNewCartAsync(ShoppingCart cart);
        Task<ShoppingCart> UpdateTotalPriceAsync(decimal totalPrice, long cartId);
    }
}