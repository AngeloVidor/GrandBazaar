using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.Interfaces.Management;
using Microsoft.AspNetCore.Http;

namespace Auth.BLL.Services.Management
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserIdFromContext()
        {
            var userId = _httpContextAccessor.HttpContext?.Items["userId"] as string;
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var id))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID.");
            }
            return id;
        }
    }
}