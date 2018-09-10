using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaasFeeGuides.Data;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models.Entities;
using SaasFeeGuides.ViewModels;


namespace SaasFeeGuides.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
   

        public AccountController(UserManager<AppUser> userManager,ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
         
            _appDbContext = appDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Registration model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new AppUser()
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            if (model.IsAdmin)
            {
                await _userManager.AddClaimAsync(userIdentity, new System.Security.Claims.Claim(Constants.Strings.JwtClaimIdentifiers.Role, Constants.Strings.JwtClaims.ApiAdminAccess));
            }
        

            return new OkResult();
        }

        [HttpDelete]
        [Authorize("User")]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idClaim = this.User.Claims.FirstOrDefault(c => c.Type == "id");

            var user = await _userManager.FindByIdAsync(idClaim.Value);

            var result = await _userManager.DeleteAsync(user);

            return new OkResult();
        }
    }
}