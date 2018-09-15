using System;
using System.IO;
using System.Threading.Tasks;
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
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");
            var activitiesEnglish = await _client.GetActivities("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivityAndSkus(authClient);
            }
        }

        [Fact]
        public async Task GetActivities()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");

         
            var activitiesEnglish = await _client.GetActivities("en");
            if(activitiesEnglish.Count==0)
            {
                await AddActivityAndSkus(authClient);
            }
            activitiesEnglish = await _client.GetActivities("en");
            var activitiesGerman = await _client.GetActivities("de");
        }
        [Fact]
        public async Task GetActivity()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");

            var activitiesEnglish = await _client.GetActivities("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivityAndSkus(authClient);
            }
         
            activitiesEnglish = await _client.GetActivities("en");
            var activityEnglish = await _client.GetActivity(activitiesEnglish[0].Id,"en");
        }
        [Fact]
        public async Task AddAndUpdateActivityThenSkus()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");

            var activitiesEnglish = await _client.GetActivities("en");
            if (activitiesEnglish.Count == 0)
            {
                await AddActivities(authClient);
            }
           
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


    }
}
