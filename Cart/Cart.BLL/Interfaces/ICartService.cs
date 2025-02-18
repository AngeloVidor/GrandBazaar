using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;

namespace Cart.BLL.Interfaces
{
    public interface ICartService
    {
        Task<ShoppingCartDto> AddNewCartAsync(ShoppingCartDto cartDto, long userId);
    }
}