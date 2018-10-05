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
            var activitiesEnglish = await _client.GetActivitiesLoc("en");
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

            await AddActivitiesAndSkusIfNeeded(authClient);
            await AddDates(authClient);

            {
                var activitySkuDateId = await authClient.AddActivitySkuDate(new ActivitySkuDate()
                {
                    ActivityName = "Allalin",
                    DateTime = new DateTime(2018, 9, 20,9,0,0)
                });
            }
        }

      

        [Fact]
        public async Task GetActivityDates()
        {
            var authClient = await AuthClient();

            await AddActivitiesAndSkusIfNeeded(authClient);
            await AddDates(authClient);
            var activitiesGerman = await _client.GetActivitiesLoc("de");
            var activity = await _client.GetActivityLoc(activitiesGerman.FirstOrDefault(a => a.Name == "Allalin").Id,"de");

            await AddCustomerBookingsIfNeeded(authClient);

            var activityDates = await _client.GetActivityDates(activity.Id,null,null);
            Assert.All(activityDates.Select(x=>x.StartDateTime).Distinct(), (d=> _dates.Contains(d)));
            Assert.Equal(4, activityDates.Count);

            Assert.Equal(new DateTime(2018, 9, 20,13,0,0), activityDates[0].EndDateTime);

            var datesLimited = await _client.GetActivityDates(activity.Skus[0].Id, _dates[0].AddDays(1), null);

            var remainingDatesExpected = _dates.Skip(1).ToList();
            Assert.Equal(3, datesLimited.Count);

            Assert.All(datesLimited.Select(x => x.StartDateTime).Distinct(), (d => remainingDatesExpected.Contains(d)));
            Assert.Equal(3,datesLimited[2].NumPersons);
            Assert.Equal(630, datesLimited[2].TotalPrice);
            Assert.Equal(0, datesLimited[2].AmountPaid);
        }

        [Fact]
        public async Task GetActivity()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activities = await authClient.GetActivities();
            Assert.True(activities.Count > 0);

            var activity = await authClient.GetActivity(activities[0].Id.Value);

            Assert.NotNull(activity);
            Assert.NotNull(activity.Equiptment);
            Assert.Equal(8, activity.Equiptment.Count);
        }
        [Fact]
        public async Task GetActivities()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activities = await authClient.GetActivities();
        }
        [Fact]
        public async Task GetActivitiesLoc()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivitiesLoc("en");
            var activitiesGerman = await _client.GetActivitiesLoc("de");

            Assert.Equal(activitiesEnglish.Count, activitiesGerman.Count);
        }
        [Fact]
        public async Task GetActivitySku()
        {
            var authClient = await AuthClient();

            await AddActivitiesAndSkusIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivitiesLoc("en");
            Assert.True(activitiesEnglish.Count > 0);

            var activityEnglish = await _client.GetActivityLoc(activitiesEnglish[0].Id, "en");

            var activitySkuEnglish = await _client.GetActivitySku(activityEnglish.Skus[0].Id, "en");

            Assert.NotNull(activitySkuEnglish);
        }

        [Fact]
        public async Task GetActivityLoc()
        {
            var authClient = await AuthClient();

            await AddActivitiesIfNeeded(authClient);

            var activitiesEnglish = await _client.GetActivitiesLoc("en");
            Assert.True(activitiesEnglish.Count > 0);

            var activityEnglish = await _client.GetActivityLoc(activitiesEnglish[0].Id,"en");
            var activityGerman = await _client.GetActivityLoc(activitiesEnglish[0].Id, "de");

            Assert.NotNull(activityEnglish);
            Assert.NotNull(activityEnglish.Equiptment);
            Assert.Equal(8, activityEnglish.Equiptment.Count);
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

        private async Task AddActivitiesIfNeeded(AuthenticatedClient authClient)
        {
            var activitiesEnglish = await _client.GetActivitiesLoc("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivities(authClient);
            }
        }





        private static List<DateTime>  _dates = new List<DateTime>()
            {
                new DateTime(2018,9,20,9,0,0),
                new DateTime(2018,10,15,9,0,0),
                new DateTime(2018,11,15,9,0,0),
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
