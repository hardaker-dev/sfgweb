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
        private readonly IEquiptmentRepository _equiptmentRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IContentRepository _contentRepository;

        public ActivityController(
            IEquiptmentRepository equiptmentRepository,
            IActivityRepository activityRepository,
            IContentRepository contentRepository,
            ICustomerRepository customerRepository)
        {
            _equiptmentRepository = equiptmentRepository;
            _activityRepository = activityRepository;
            _contentRepository = contentRepository;
            _customerRepository = customerRepository;
        }

      
        [Authorize("Admin")]
        [AllowAnonymous]
        [HttpGet("{activityId:int}/dates")]
        public async Task<IActionResult> GetActivityDates(int activityId, DateTime? dateFrom, DateTime? dateTo)
        {
            var dates = await _activityRepository.SelectActivityDates(new[] { activityId }, dateFrom, dateTo);

            if(User.HasPolicy("Admin"))
            {
                return new OkObjectResult(dates.Select(Mapping.MapAdmin));
            }

            return new OkObjectResult(dates.Select(Mapping.Map));
        }
        [Authorize("Admin")]
        [AllowAnonymous]
        [HttpGet("dates")]
        public async Task<IActionResult> GetAllActivityDates(DateTime? dateFrom, DateTime? dateTo)
        {
            var activityDates = await _activityRepository.SelectActivityDates(new int[0],dateFrom,dateTo);
            if (User.HasPolicy("Admin"))
            {
                return new OkObjectResult(activityDates.Select(Mapping.MapAdmin));
            }
            return new OkObjectResult(activityDates.Select(Mapping.Map));
        }

        [HttpGet("sku/{activitySkuId:int}")]
        public async Task<IActionResult> GetActivitySku(int activitySkuId,string locale)
        {
            var activitySku = await _activityRepository.SelectActivitySku(activitySkuId, locale ?? "en");

            return new OkObjectResult(activitySku);
        }
        [Authorize("Admin")]
        [HttpDelete("sku/date/{activitySkuDateId:int}/customer/{customerEmail}")]
        public async Task<IActionResult> DeleteActivitySkuDateCustomer(int activitySkuDateId,string customerEmail)
        {
            await _customerRepository.DeleteCustomerBooking(activitySkuDateId, customerEmail);
           
            return new OkResult();
        }
        [Authorize("Admin")]
        [HttpPost("sku/date")]
        public async Task<IActionResult> AddActivitySkuDate(ViewModels.NewActivitySkuDate activitySkuDate)
        {
            var id = await _activityRepository.InsertActivitySkuDate(activitySkuDate);
            foreach(var customerBooking in activitySkuDate.CustomerBookings ?? new ViewModels.CustomerBooking[0])
            {
                await _customerRepository.UpsertCustomerBooking(customerBooking.Map());
            }
            return new OkObjectResult(id);
        }

        [Authorize("Admin")]
        [HttpPut("sku")]
        public async Task<IActionResult> AddOrUpdateActivitySku(ViewModels.ActivitySku activitySku)
        {
            var activitySkuModel = await activitySku.Map(_contentRepository);

            var id = await _activityRepository.UpsertActivitySku(activitySkuModel);

            return new OkObjectResult(id);
        }       

        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddActivity(ViewModels.Activity activity)
        {
            var activityModel = await activity.Map(null, _contentRepository);

            var activityId = await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);
            await InsertEquiptment(activity.Equiptment, activityId);

            return new OkObjectResult(activityId);
        }

        private async Task InsertEquiptment(IList<ViewModels.ActivityEquiptment> equiptments, int activityId)
        {
            if (equiptments == null)
                return;

            foreach (var equiptment in equiptments)
            {
                var equiptmentId = await _equiptmentRepository.UpsertEquiptment(equiptment.Map(_contentRepository));
                await _equiptmentRepository.InsertActivityEquiptment(new Models.ActivityEquiptment()
                {
                    ActivityId = activityId,
                    EquiptmentId = equiptmentId,
                    GuideOnly = equiptment.GuideOnly
                });
            }
        }

        [Authorize("Admin")]
        [HttpPatch]
        public async Task<IActionResult> UpdateActivity(ViewModels.Activity activity)
        {
            var activityId = await _activityRepository.FindActivityByName(activity.Name);
            var activityModel = await activity.Map(activityId,_contentRepository);

           
            await _activityRepository.UpsertActivity(activityModel);
            EnsureMatchingActivityName(activity);
            await UpsertSkus(activity.Skus);

            return new OkObjectResult(activityId);
        }

        [Authorize("Admin")]
        [HttpPatch("sku/date")]
        public async Task<IActionResult> UpdateActivitySkuDate(ViewModels.ActivitySkuDate activitySkuDate)
        {
            await _activityRepository.UpdateActivitySkuDate(activitySkuDate);
           

            return new OkResult();
        }
        [Authorize("Admin")]
        [HttpGet("{activityId:int}/edit")]
        public async Task<IActionResult> GetActivity(int activityId)
        {
            var activity = await _activityRepository.SelectActivity(activityId);

            return new OkObjectResult(await activity.Map(_contentRepository));
        }
        [Authorize("Admin")]
        [HttpGet("edit")]
        public async Task<IActionResult> GetActivities()
        {
            var activities = await _activityRepository.SelectActivities();

            var mapped =  await Task.WhenAll( activities.Select(a => a.Map(_contentRepository)));
            return new OkObjectResult(mapped);
        }

        [Authorize("Admin")]
        [HttpDelete("sku/date/{activitySkuDateId:int}")]
        public async Task<IActionResult> DeleteActivitySkuDate(int activitySkuDateId)
        {
            await _activityRepository.DeleteActivitySkuDate(activitySkuDateId);


            return new OkResult();
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
       
        private async Task<IEnumerable<(string name, int id)>> UpsertSkus(IList<ViewModels.ActivitySku> activitySkus)
        {
            if (activitySkus == null)
                return Enumerable.Empty<(string name, int id)>();


            var results = await Task.WhenAll(activitySkus.Select(sku =>
            {
                return sku.Map(_contentRepository)
                .ContinueWith(async activitySkuModel =>
                {
                    var activitySkuId = await _activityRepository.UpsertActivitySku(await activitySkuModel);
                    foreach (var priceOption in sku.PriceOptions ?? Enumerable.Empty<ViewModels.ActivitySkuPrice>())
                    {
                        priceOption.ActivitySkuId = activitySkuId;
                        await _activityRepository.UpsertActivitySkuPrice(await priceOption.Map(_contentRepository));
                    }

                    return (sku.Name,activitySkuId);
                });
            }));
            //unwrap
            return await Task.WhenAll(results.Select(y => y));
        }
    }
}