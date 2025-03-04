using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;

namespace Cart.BLL.Interfaces.Management
{
    public interface ICartManagementService
    {
        Task<CreatorItemDto> AddItemIntoCartAsync(CreatorItemDto item, long userId);
        Task<CreatorItemDto> DeleteItemFromCartAsync(long cartId, long productId, int quantity);
        Task<IEnumerable<CreatorItemDto>> GetItemsFromCartAsync(long cartId);

    }
}