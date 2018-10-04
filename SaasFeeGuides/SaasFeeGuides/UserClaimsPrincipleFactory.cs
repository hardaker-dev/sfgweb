using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SaasFeeGuides.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaasFeeGuides
{
 

    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public ClaimsTransformer(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(x => x.Type == "id");
            var user = await _userManager.FindByIdAsync(id.Value);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var claimsIdentity = (ClaimsIdentity)principal.Identity;
                claimsIdentity.AddClaims(claims);
            }
            return principal;
        }
    }
}
