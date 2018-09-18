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
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(
            UserManager<AppUser> userManager,
            ICustomerRepository accountRepository)
        {
            _userManager = userManager;      
            _customerRepository = accountRepository;
        }
        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCustomer(ViewModels.Customer customer)
        {
            var customerModel = customer.Map();

            var customerId = await _customerRepository.UpsertCustomer(customerModel);
          

            return new OkObjectResult(customerId);
        }

        [Authorize("Admin")]
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {

            var customers = await _customerRepository.SelectCustomers();


            return new OkObjectResult(customers);
        }

        [Authorize("Admin")]
        [HttpPost("booking/historic")]
        public async Task<IActionResult> AddHistoricCustomerBooking(ViewModels.HistoricCustomerBooking customerBooking)
        {
            var customerModel = customerBooking.Map();

            var customerId = await _customerRepository.UpsertCustomerBooking(customerModel);


            return new OkObjectResult(customerId);
        }
    }
}