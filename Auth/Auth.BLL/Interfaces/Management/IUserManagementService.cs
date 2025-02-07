using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.BLL.Interfaces.Management
{
    public interface IUserManagementService
    {
        long GetUserIdFromContext();
    }
}