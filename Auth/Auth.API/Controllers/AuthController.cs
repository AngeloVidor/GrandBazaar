using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;
using Auth.BLL.Interfaces;
using Auth.BLL.Interfaces.Management;
using Auth.BLL.Interfaces.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBearerTokenManagement _bearerTokenManagement;
        private readonly IUserManagementService _userManagementService;


        public AuthController(IAuthService authService, IBearerTokenManagement bearerTokenManagement, IUserManagementService userManagementService)
        {
            _authService = authService;
            _bearerTokenManagement = bearerTokenManagement;
            _userManagementService = userManagementService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUserAsync([FromBody] RegistrationDto registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedUser = await _authService.AddNewUserAsync(registration);
                return Ok(addedUser);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authService.SignInAsync(email, password);
                var token = await _bearerTokenManagement.GenerateTokenAsync(user);
                return Ok(new { Token = token, User = user });
            }
            catch (Exception ex)
            {
                return StatusCode(50, ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
                var user = _userManagementService.GetUserIdFromContext();
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}