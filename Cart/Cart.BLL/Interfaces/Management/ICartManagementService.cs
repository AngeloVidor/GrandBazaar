using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;

namespace Cart.BLL.Interfaces.Management
{
    public interface ICartManagementService
    {
        Task<ItemDto> AddItemIntoCartAsync(ItemDto item);
    }
}