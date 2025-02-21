using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.Domain.Entities;

namespace Products.DAL.Interfaces.Management
{
    public interface IProductManagementRepository
    {
        Task<Product> ValidateProductAsync(int quantity, long productId);
    }
}