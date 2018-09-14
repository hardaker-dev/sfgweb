using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaasFeeGuides.Data;
using SaasFeeGuides.Models;
using SaasFeeGuides.ViewModels;

namespace SaasFeeGuides.Controllers
{
    [Authorize("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IContentRepository _contentRepository;

        public ActivityController(
            IActivityRepository activityRepository,
            IContentRepository contentRepository)
        {
            _activityRepository = activityRepository;
            _contentRepository = contentRepository;
        }

        [HttpPost("sku")]
        public async Task<IActionResult> CreateActivitySku(ViewModels.ActivitySku activitySku)
        {
            var activityModel = new Models.ActivitySku
            {
                ActivityName = activitySku.ActivityName,
                Name = activitySku.Name,
                TitleContentId =  _contentRepository.InsertContent(activitySku.TitleContent),
                DescriptionContentId =  _contentRepository.InsertContent(activitySku.DescriptionContent),
                PricePerPerson = activitySku.PricePerPerson,
                MinPersons = activitySku.MinPersons,
                MaxPersons = activitySku.MaxPersons,
                AdditionalRequirementsContentId =  _contentRepository.InsertContent(activitySku.AdditionalRequirementsContent),
                DurationDays = activitySku.DurationDays,
                DurationHours = activitySku.DurationHours,
                WebContentId =  _contentRepository.InsertContent(activitySku.WebContent),
            };

            var id = await _activityRepository.UpsertActivitySku(activityModel);

            return new OkObjectResult(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(ViewModels.Activity activity)
        {
            var activityModel = new Models.Activity
            {
                TitleContentId =  _contentRepository.InsertContent(activity.TitleContent),
                MenuImageContentId =  _contentRepository.InsertContent( activity.MenuImageContentId),
                ImageContentIds =  _contentRepository.InsertContent(activity.ImageContentIds),
                VideoContentIds =  _contentRepository.InsertContent(activity.VideoContentIds),
                Name = activity.Name,
                DescriptionContentId =  _contentRepository.InsertContent(activity.DescriptionContent),
                IsActive = activity.IsActive
            };

            var id = await _activityRepository.UpsertActivity(activityModel);

            return new OkObjectResult(id);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateActivity(ViewModels.Activity activity)
        {
            var activityId = await _activityRepository.FindActivityByName(activity.Name);
            var activityModel = new Models.Activity
            {
                Id = activityId,
                TitleContentId =  _contentRepository.InsertContent(activity.TitleContent),
                MenuImageContentId =  _contentRepository.InsertContent( activity.MenuImageContentId),
                ImageContentIds =  _contentRepository.InsertContent(activity.ImageContentIds),
                VideoContentIds =  _contentRepository.InsertContent(activity.VideoContentIds),
                Name = activity.Name,
                DescriptionContentId =  _contentRepository.InsertContent(activity.DescriptionContent),
                IsActive = activity.IsActive
            };

            var id = await _activityRepository.UpsertActivity(activityModel);

            return new OkObjectResult(id);
        }

        [HttpPatch("sku")]
        public async Task<IActionResult> UpdateActivitySku(ViewModels.ActivitySku activitySku)
        {
            var activitySkuId = await _activityRepository.FindActivitySkuByName(activitySku.Name);
            var activityModel = new Models.ActivitySku
            {
                Id = activitySkuId,
                ActivityName = activitySku.ActivityName,
                Name = activitySku.Name,
                TitleContentId =  _contentRepository.InsertContent(activitySku.TitleContent),
                DescriptionContentId =  _contentRepository.InsertContent(activitySku.DescriptionContent),
                PricePerPerson = activitySku.PricePerPerson,
                MinPersons = activitySku.MinPersons,
                MaxPersons = activitySku.MaxPersons,
                AdditionalRequirementsContentId =  _contentRepository.InsertContent(activitySku.AdditionalRequirementsContent),
                DurationDays = activitySku.DurationDays,
                DurationHours = activitySku.DurationHours,
                WebContentId =  _contentRepository.InsertContent(activitySku.WebContent),
            };

            var id = await _activityRepository.UpsertActivitySku(activityModel);

            return new OkObjectResult(id);
        }
    }
}