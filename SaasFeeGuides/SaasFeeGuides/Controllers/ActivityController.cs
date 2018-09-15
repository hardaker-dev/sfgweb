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
using SaasFeeGuides.Helpers;
namespace SaasFeeGuides.Controllers
{
   
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

        [HttpGet("sku/{activitySkuId:int}/date")]
        public async Task<IActionResult> GetActivitySkuDates(int activitySkuId,DateTime? dateFrom, DateTime? dateTo)
        {
            var dates = await _activityRepository.SelectActivitySkuDates(activitySkuId, dateFrom, dateTo);

            return new OkObjectResult(dates);
        }

        [HttpGet("sku/{activitySkuId:int}")]
        public async Task<IActionResult> GetActivitySku(int activitySkuId,string locale)
        {

            var activitySku = await _activityRepository.SelectActivitySku(activitySkuId, locale ?? "en");

            return new OkObjectResult(activitySku);
        }

        [Authorize("Admin")]
        [HttpPost("sku/date")]
        public async Task<IActionResult> AddActivitySkuDate(ViewModels.ActivitySkuDate activitySkuDate)
        {
            var id = await _activityRepository.InsertActivitySkuDate(activitySkuDate);

            return new OkObjectResult(id);
        }

        [Authorize("Admin")]
        [HttpPut("sku")]
        public async Task<IActionResult> AddOrUpdateActivitySku(ViewModels.ActivitySku activitySku)
        {
            Models.ActivitySku activitySkuModel = activitySku.Map(_contentRepository);

            var id = await _activityRepository.UpsertActivitySku(activitySkuModel);

            return new OkObjectResult(id);
        }

        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddActivity(ViewModels.Activity activity)
        {
            var activityModel = activity.Map(null,_contentRepository);

            var activityId = await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);

            return new OkObjectResult(activityId);
        }

        [Authorize("Admin")]
        [HttpPatch]
        public async Task<IActionResult> UpdateActivity(ViewModels.Activity activity)
        {
            var activityId = await _activityRepository.FindActivityByName(activity.Name);
            var activityModel = activity.Map(activityId,_contentRepository);

           
            await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);

            return new OkObjectResult(activityId);
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities(string locale)
        {
            var activities = await _activityRepository.SelectActivities(locale ?? "en");
           

            return new OkObjectResult(activities);
        }

        [HttpGet("{activityId:int}")]
        public async Task<IActionResult> GetActivity(int activityId, string locale)
        {
            var activity = await _activityRepository.SelectActivity(activityId, locale ?? "en");

            return new OkObjectResult(activity.Map());
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
       
        private async Task<IEnumerable<(string name, int id)>> UpsertSkus(ViewModels.ActivitySku[] activitySkus)
        {
            if (activitySkus == null)
                return Enumerable.Empty<(string name, int id)>();

            return await Task.WhenAll(activitySkus.Select(sku =>
            {
                var activitySkuModel = sku.Map(_contentRepository);

                return _activityRepository.UpsertActivitySku(activitySkuModel).ContinueWith(skuTask => (sku.Name, skuTask.Result));
            }));
        }
    }
}