﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SaasFeeGuides.Auth;
using SaasFeeGuides.Data;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models;
using SaasFeeGuides.Models.Entities;
using SaasFeeGuides.ViewModels;
using Customer = SaasFeeGuides.ViewModels.Customer;

namespace SaasFeeGuides.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICustomerRepository _customerRepository;


        public AuthController(UserManager<AppUser> userManager, 
            IJwtFactory jwtFactory, 
            IOptions<JwtIssuerOptions> jwtOptions,
            ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _customerRepository = customerRepository;

        }
        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody]Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
          //  var customer = await _customerRepository.SelectCustomerByUserId(identity.Claims.Single(c => c.Type == "id").Value);
            // Serialize and return the response
            var response = new User
            {
            //    Customer = customer.Map(),
                AuthToken = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return new OkObjectResult(response);
        }

       

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}