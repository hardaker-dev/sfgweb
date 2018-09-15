﻿using System;
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

        [HttpPut("sku")]
        public async Task<IActionResult> AddOrUpdateActivitySku(ViewModels.ActivitySku activitySku)
        {
            Models.ActivitySku activityModel = MapToModel(activitySku);

            var id = await _activityRepository.UpsertActivitySku(activityModel);

            return new OkObjectResult(id);
        }      

        [HttpPost]
        public async Task<IActionResult> AddActivity(ViewModels.Activity activity)
        {
            var activityModel = MapToModel(activity, null);

            var activityId = await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);

            return new OkObjectResult(activityId);
        }        

        [HttpPatch]
        public async Task<IActionResult> UpdateActivity(ViewModels.Activity activity)
        {
            var activityId = await _activityRepository.FindActivityByName(activity.Name);
            var activityModel = MapToModel(activity, activityId);

           
            await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);

            return new OkObjectResult(activityId);
        }

        private void EnsureMatchingActivityName(ViewModels.Activity activity)
        {
            if (activity.Skus == null)
                return;
            foreach(var activitySku in activity.Skus)
            {
                activitySku.ActivityName = activity.Name;
            }
        }

        private async Task<IEnumerable<(string name,int id)>> UpsertSkus(ViewModels.ActivitySku[] activitySkus)
        {
            if (activitySkus == null)
                return Enumerable.Empty<(string name, int id)>();

            return await Task.WhenAll(activitySkus.Select(sku =>
                   {
                       Models.ActivitySku activitySkuModel = MapToModel(sku);
                        
                       return  _activityRepository.UpsertActivitySku(activitySkuModel).ContinueWith(skuTask => (sku.Name,skuTask.Result));
                   }));
        }

        private Models.Activity MapToModel(ViewModels.Activity activity, int? activityId)
        {
            return new Models.Activity
            {
                Id = activityId,
                TitleContentId = _contentRepository.InsertContent(activity.TitleContent),
                MenuImageContentId = _contentRepository.InsertContent(activity.MenuImageContentId),
                ImageContentIds = _contentRepository.InsertContent(activity.ImageContentIds),
                VideoContentIds = _contentRepository.InsertContent(activity.VideoContentIds),
                Name = activity.Name,
                DescriptionContentId = _contentRepository.InsertContent(activity.DescriptionContent),
                IsActive = activity.IsActive
            };
        }

        private Models.ActivitySku MapToModel(ViewModels.ActivitySku activitySku)
        {
            return new Models.ActivitySku
            {
                ActivityName = activitySku.ActivityName,
                Name = activitySku.Name,
                TitleContentId = _contentRepository.InsertContent(activitySku.TitleContent),
                DescriptionContentId = _contentRepository.InsertContent(activitySku.DescriptionContent),
                PricePerPerson = activitySku.PricePerPerson,
                MinPersons = activitySku.MinPersons,
                MaxPersons = activitySku.MaxPersons,
                AdditionalRequirementsContentId = _contentRepository.InsertContent(activitySku.AdditionalRequirementsContent),
                DurationDays = activitySku.DurationDays,
                DurationHours = activitySku.DurationHours,
                WebContentId = _contentRepository.InsertContent(activitySku.WebContent),
            };
        }

    }
}