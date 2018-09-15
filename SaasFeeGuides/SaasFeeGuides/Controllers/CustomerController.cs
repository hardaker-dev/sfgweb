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
    public class CustomerController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ICustomerRepository _accountRepository;

        public CustomerController(
            UserManager<AppUser> userManager,
            ICustomerRepository accountRepository)
        {
            _userManager = userManager;      
            _accountRepository = accountRepository;
        }
      
    }
}