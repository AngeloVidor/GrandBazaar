using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.BLL.Interfaces.Cart
{
    public interface IUserRequestHandler
    {
        Task HandleUserRequest();
    }
}