using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sellers.BLL.DTOs;
using Sellers.BLL.Interfaces;

namespace Sellers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public SellerProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("Profile")]
        public async Task<IActionResult> AddNewProfileAsync([FromForm] SellerDetailsDto sellerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = HttpContext.Items["userId"]?.ToString();
                sellerDetails.User_Id = long.Parse(userId);

                var profile = await _profileService.AddSellerProfileAsync(sellerDetails);
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("myProfile")]
        public async Task<IActionResult> MyProfile()
        {
            var userId = long.Parse(HttpContext.Items["userId"]?.ToString());

            try
            {
                var profile = await _profileService.GetMyProfileAsync(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}