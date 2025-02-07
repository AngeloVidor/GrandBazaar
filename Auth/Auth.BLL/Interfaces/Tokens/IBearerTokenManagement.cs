using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;

namespace Auth.BLL.Interfaces.Tokens
{
    public interface IBearerTokenManagement
    {
        Task<string> GenerateTokenAsync(LoginDto user);
    }
}