using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Payments.BLL.Interfaces.Cart;
using Payments.BLL.Messaging.Cart.Interfaces;

namespace Payments.BLL.Services.Cart
{
    public class UserRequestHandler : IUserRequestHandler
    {
        private readonly IProductRequestPublisher _productRequestPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRequestHandler(IProductRequestPublisher productRequestPublisher, IHttpContextAccessor httpContextAccessor)
        {
            _productRequestPublisher = productRequestPublisher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task HandleUserRequest()
        {
            long userId = long.Parse(_httpContextAccessor.HttpContext.Items["userId"].ToString());
            if (userId <= 0)
            {
                throw new UnauthorizedAccessException("Access denied. Please log in to continue.");
            }
            var response = await _productRequestPublisher.Publish(userId);

            foreach (var item in response)
            {
                Console.WriteLine(item.ProductName);
                Console.WriteLine(item.Price);
                Console.WriteLine(item.Quantity);
                Console.WriteLine(item.Product_Id);
                Console.WriteLine(item.Cart_Id);
                Console.WriteLine(item.Item_Id);
                Console.WriteLine(response.Count());
            }
        }
    }
}