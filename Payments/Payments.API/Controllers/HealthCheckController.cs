using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payments.BLL.Interfaces.Cart;

namespace Payments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IUserRequestHandler _userRequestHandler;

        public HealthCheckController(IUserRequestHandler userRequestHandler)
        {
            _userRequestHandler = userRequestHandler;
        }

        [HttpPost("post")]
        public async Task<IActionResult> Run()
        {
            try
            {
                await _userRequestHandler.HandleUserRequest();
                return Ok("sent");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}