﻿using System.Linq;
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
        private readonly IActivityRepository _activityRepository;

        public CustomerController(
            UserManager<AppUser> userManager,
            ICustomerRepository accountRepository,
            IActivityRepository activityRepository)
        {
            _userManager = userManager;      
            _customerRepository = accountRepository;
            _activityRepository = activityRepository;
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


            return new OkObjectResult(customers.Select(Mapping.Map));
        }
        [Authorize("Admin")]
        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetCustomer(int customerId)
        {

            var customer = await _customerRepository.SelectCustomer(customerId);


            return new OkObjectResult(customer);
        }

        [Authorize("Admin")]
        [HttpPost("booking/historic")]
        public async Task<IActionResult> AddHistoricCustomerBooking(ViewModels.HistoricCustomerBooking customerBooking)
        {
            var customerModel = customerBooking.Map();

            var customerBookingId = await _customerRepository.UpsertCustomerBooking(customerModel);


            return new OkObjectResult(customerBookingId);
        }

        [Authorize("Admin")]
        [HttpPost("booking")]
        public async Task<IActionResult> AddCustomerBooking(ViewModels.CustomerBooking customerBooking)
        {
            var customerModel = customerBooking.Map();

            var activitySkuId = await this._activityRepository.FindActivitySkuByName(customerBooking.ActivitySkuName);
            var activitySku = await this._activityRepository.SelectActivitySku(activitySkuId, "en");
            customerModel.PriceAgreed = activitySku.PricePerPerson * customerBooking.NumPersons;

            var customerBookingId = await _customerRepository.UpsertCustomerBooking(customerModel);


            return new OkObjectResult(customerBookingId);
        }

        [Authorize("Admin")]
        [HttpGet("{customerId:int}/booking")]
        public async Task<IActionResult> GetCustomerBookings(int customerId)
        {
            var customerBookings = await _customerRepository.SelectCustomerBookings(customerId);
            return new OkObjectResult(customerBookings);
        }
    }
}