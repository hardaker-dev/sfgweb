using System;
using System.Collections.Generic;
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
    public class GuideController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IGuideRepository _guideRepository;
        private readonly IActivityRepository _activityRepository;

        public GuideController(
            UserManager<AppUser> userManager,
            IGuideRepository guideRepository,
            IActivityRepository activityRepository)
        {
            _userManager = userManager;      
            _guideRepository = guideRepository;
            _activityRepository = activityRepository;
        }
        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddGuide(ViewModels.Guide guide)
        {
            var customerModel = guide.Map();
            var guideId = await _guideRepository.UpsertGuide(customerModel);         

            return new OkObjectResult(guideId);
        }

        [Authorize("Admin")]
        [HttpGet]
        public async Task<IActionResult> GetGuides(string searchText)
        {
            searchText = searchText?.Trim();
            IEnumerable<Models.Guide> guides = null;
            if(string.IsNullOrEmpty(searchText))
            {
                guides = await _guideRepository.SelectGuides(null, null, null);
            }
            else if (searchText.Contains("@"))
            {
                guides = await _guideRepository.SelectGuides(searchText, null, null);
            }
            else if (searchText.Contains(" "))
            {
                var split = searchText.Split(' ');

                var firstName = split[0];
                var lastName = split[1];
                guides = await _guideRepository.SelectGuides(null, firstName, lastName);
            }
            else
            {
                var emailMatches = _guideRepository.SelectGuides(searchText, null, null);
                var firstNameMatches = _guideRepository.SelectGuides(null, searchText, null);
                var lastNameMatches = _guideRepository.SelectGuides(null, null, searchText);
                guides = (await firstNameMatches).Concat(await lastNameMatches).Concat(await emailMatches).Distinct(new Models.GuideComparer());
            }

            return new OkObjectResult(guides.Select(Mapping.Map));
        }

        [Authorize("Admin")]
        [HttpGet("{guideId:int}")]
        public async Task<IActionResult> GetGuide(int guideId)
        {

            var guide = await _guideRepository.SelectGuide(guideId);


            return new OkObjectResult(guide);
        }

    
      

  

     
    }
}