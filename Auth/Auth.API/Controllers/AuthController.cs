using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;
using Auth.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
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
    }
}