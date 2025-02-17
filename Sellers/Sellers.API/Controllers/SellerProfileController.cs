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
        private readonly IProfileManagementService _profileManagementService;

        public SellerProfileController(IProfileService profileService, IProfileManagementService profileManagementService)
        {
            _profileService = profileService;
            _profileManagementService = profileManagementService;
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

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdatedSellerDetailsDto sellerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = long.Parse(HttpContext.Items["userId"].ToString());
                sellerDetails.Seller_Id = await _profileManagementService.GetSellerProfileIdByUserIdAsync(userId);

                var updatedProfile = await _profileService.UpdateSellerProfileAsync(sellerDetails);
                return Ok(updatedProfile);
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