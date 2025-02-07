using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> AddNewProfileAsync([FromBody] SellerDetailsDto sellerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var profile = await _profileService.AddSellerProfileAsync(sellerDetails);
                return Ok(profile);
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
    }
}