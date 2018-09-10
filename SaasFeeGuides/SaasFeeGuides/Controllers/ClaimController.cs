using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models.Entities;
using SaasFeeGuides.ViewModels;

namespace SaasFeeGuides.Controllers
{
    [Authorize(Policy ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;


        public ClaimController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> PostClaim([FromBody]AppClaim claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Claims.Any(x => x.Type == claim.ClaimType && x.Value == claim.ClaimValue))
            {
                return new OkResult();
            }

            var idClaim = this.User.Claims.FirstOrDefault(c => c.Type == "id");

            var user = await _userManager.FindByIdAsync(idClaim.Value);
            var allClaims = await _userManager.GetClaimsAsync(user);

            await _userManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue));
           


            return new OkResult();
        }
    }
}