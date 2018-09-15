using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RiskFirst.RestClient;
using SaasFeeGuides.IntegrationTests.TestFramework;
using SaasFeeGuides.RestClient;
using SaasFeeGuides.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests
{
    [Collection("Database collection")]
    public class ActivityTestFixture : BaseTestFixture
    {
        public ActivityTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }
        [Fact]
        public async Task AddAndUpdateActivityAndSkus()
        {
            var authClient = await AuthClient();
            var activitiesEnglish = await _client.GetActivities("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivityAndSkus(authClient);
            }
        }      

        [Fact]
        public async Task AddActivitySkuDate_DoesntExist()
        {
            var authClient = await AuthClient();
            await AddActivitiesIfNeeded(authClient);

            try
            {
                var activitySkuDateId = await authClient.AddActivitySkuDate(new ActivitySkuDate()
                {
                    ActivityName = "NotAllalin",
                    DateTime = new DateTime(2018, 9, 20)
                });
            }
            catch (RestResponseException e)
            {
                Assert.Equal("Cannot find activity with name 'NotAllalin'", e.Body);
            }

        }


        [Fact]
        public async Task AddActivitySkuDate()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);
            await AddDates(authClient);

            {
                var activitySkuDateId = await authClient.AddActivitySkuDate(new ActivitySkuDate()
                {
                    ActivityName = "Allalin",
                    DateTime = new DateTime(2018, 9, 20)
                });
            }
        }

      

        [Fact]
        public async Task GetActivitySkuDates()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);
            await AddDates(authClient);
            var activitiesGerman = await _client.GetActivities("de");
            var activity = await _client.GetActivity(activitiesGerman.FirstOrDefault(a => a.Name == "Allalin").Id,"de");
            var dates = await _client.GetActivitySkuDates(activity.Skus[0].Id,null,null);
            Assert.All(dates,(d=> _dates.Contains(d)));
            Assert.Equal(3, dates.Count);

            var datesLimited = await _client.GetActivitySkuDates(activity.Skus[0].Id, _dates[0].AddDays(1), null);

            var remainingDatesExpected = _dates.Skip(1).ToList();
            Assert.Equal(2, datesLimited.Count);
            Assert.All(dates, (d => remainingDatesExpected.Contains(d)));
        }

        [Fact]
        public async Task GetActivities()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivities("en");
            var activitiesGerman = await _client.GetActivities("de");

            Assert.Equal(activitiesEnglish.Count, activitiesGerman.Count);
        }
        [Fact]
        public async Task GetActivitySku()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivities("en");
            Assert.True(activitiesEnglish.Count > 0);

            var activityEnglish = await _client.GetActivity(activitiesEnglish[0].Id, "en");

            var activitySkuEnglish = await _client.GetActivitySku(activityEnglish.Skus[0].Id, "en");

            Assert.NotNull(activitySkuEnglish);
        }

        [Fact]
        public async Task GetActivity()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivities("en");
            Assert.True(activitiesEnglish.Count > 0);

            var activityEnglish = await _client.GetActivity(activitiesEnglish[0].Id,"en");

            Assert.NotNull(activityEnglish);
        }
        [Fact]
        public async Task AddAndUpdateActivityThenSkus()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            await UpdateActivities(authClient);
            await AddOrUpdatectivitySkus(authClient);
            await AddOrUpdatectivitySkus(authClient);
        }

        private static async Task AddActivityAndSkus(AuthenticatedClient authClient)
        {
            var activities = BuildActivityAndSkus();
            foreach (var activity in activities)
            {
                await authClient.AddActivity(activity);
            }
        }

        private static async Task AddActivities(AuthenticatedClient authClient)
        {
            var activities = BuildActivities();
            foreach (var activity in activities)
            {
                await authClient.AddActivity(activity);
            }
        }

        private static async Task UpdateActivities(AuthenticatedClient authClient)
        {
            var activities = BuildActivities();
            foreach (var activity in activities)
            {
                await authClient.UpdateActivity(activity);
            }
        }
        private static async Task AddOrUpdatectivitySkus(AuthenticatedClient authClient)
        {
            var activitySkus = BuildActivitySkus();
            foreach (var activitySku in activitySkus)
            {
                await authClient.AddOrUpdateActivitySku(activitySku);
            }
        }
        private static Activity[] BuildActivityAndSkus()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Activity[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.ActivitiesAndSkus);
        }
        private static ActivitySku[] BuildActivitySkus()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ActivitySku[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.ActivitySkus);
        }
        private static Activity[] BuildActivities()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Activity[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.Activities);
        }

        private async Task<AuthenticatedClient> AuthClient()
        {
            return await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");
        }


        private async Task AddActivitiesIfNeeded(AuthenticatedClient authClient)
        {
            var activitiesEnglish = await _client.GetActivities("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivityAndSkus(authClient);
            }
        }
        private static List<DateTime>  _dates = new List<DateTime>()
            {
                new DateTime(2018,9,20),
                new DateTime(2018,10,15),
                new DateTime(2018,11,15),
            };
        private static async Task AddDates(AuthenticatedClient authClient)
        {
            
            foreach (var date in _dates)
            {
                var activitySkuDateId = await authClient.AddActivitySkuDate(new ActivitySkuDate()
                {
                    ActivityName = "Allalin",
                    DateTime = date
                });

                activitySkuDateId = await authClient.AddActivitySkuDate(new ActivitySkuDate()
                {
                    ActivityName = "AllalinEast",
                    DateTime = date
                });
            }
        }
    }
}
