using SaasFeeGuides.IntegrationTests.TestFramework;
using SaasFeeGuides.RestClient;
using SaasFeeGuides.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests
{
    public class BaseTestFixture : ServiceIntegrationTestBase<TestStartup>
    {
        protected readonly Client _client;
        private AuthenticatedClient _authenticatedClient;
        protected async Task<AuthenticatedClient> AuthClient()
        {
            if (_authenticatedClient == null)
            {
                var loginResponse = await Login("testadmin", "password", "test.admin@sfg.ch");
                _authenticatedClient = new AuthenticatedClient(ServiceUri, loginResponse.AuthToken);
            }
            return _authenticatedClient;
        }
       
        public BaseTestFixture(ITestOutputHelper output)
            : base(output, 5000, 6000)
        {
            this._client = new Client(ServiceUri);
        }

        private async Task<User> Login(string username, string password, string email)
        {
            try
            {
                await _client.AddAccount(new ViewModels.CustomerAccount()
                {
                    Email = email,
                    FirstName = "james",
                    LastName = "hardaker",
                    Password = password,
                    Username = username,
                    IsAdmin = true,
                    DateOfBirth = new DateTime(1984, 10, 31),
                    PhoneNumber = "+447495605347",
                    Address = "Saas Fee"
                });
            }
            catch (Exception e)
            {

            }
            var loginResponse = await _client.Login(new ViewModels.Credentials()
            {
                UserName = username,
                Password = password
            });
            return loginResponse;
        }

        protected static async Task<IList<int>> AddCustomersIfNeeded(AuthenticatedClient authClient)
        {
            var customers = await authClient.GetCustomers();
            var ids = new List<int>();
            if (customers.Count <= 1)
            {
                ids.Add(await authClient.AddCustomer(new Customer()
                {
                    Address = "London",
                    DateOfBirth = new DateTime(1984, 10, 31),
                    Email = "dude@gmail.com",
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "+447567123456"
                }));

                ids.Add(await authClient.AddCustomer(new Customer()
                {
                    Address = "Bristol",
                    DateOfBirth = new DateTime(1984, 10, 31),
                    Email = "gal@gmail.com",
                    FirstName = "Louise",
                    LastName = "Smith",
                    PhoneNumber = "+447567123456"
                }));
            }
            return ids;
        }

        protected async Task AddActivitiesAndSkusIfNeeded(AuthenticatedClient authClient)
        {
            var activitiesEnglish = await _client.GetActivitiesLoc("en");

            if (activitiesEnglish.Count == 0)
            {
                await AddActivityAndSkus(authClient);
            }
            else if (activitiesEnglish[0].Skus == null || !activitiesEnglish[0].Skus.Any())
            {
                await AddOrUpdatectivitySkus(authClient);
            }
        }

        protected static async Task AddCustomerBookingsIfNeeded(AuthenticatedClient authClient)
        {
            var newIds = await AddCustomersIfNeeded(authClient);
            if (newIds.Any())
            {
                var bookings = new List<CustomerBooking>()
            {
                new CustomerBooking()
                {
                    ActivitySkuName = "Allalin",
                    CustomerEmail = "dude@gmail.com",
                    DateTime = new DateTime(2018,12,31,9,0,0),
                    NumPersons = 3
                }
            };
                foreach (var booking in bookings)
                {
                    await authClient.AddCustomerBooking(booking);
                }
            }
        }
        protected static async Task AddActivityAndSkus(AuthenticatedClient authClient)
        {
            var activities = BuildActivityAndSkus();
            foreach (var activity in activities)
            {
                await authClient.AddActivity(activity);
            }
        }
        protected static async Task AddOrUpdatectivitySkus(AuthenticatedClient authClient)
        {
            var activitySkus = BuildActivitySkus();
            foreach (var activitySku in activitySkus)
            {
                await authClient.AddOrUpdateActivitySku(activitySku);
            }
        }

        protected static Activity[] BuildActivityAndSkus()
        {
            var equiptment = BuildEquiptment();
            var activities = Newtonsoft.Json.JsonConvert.DeserializeObject<Activity[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.ActivitiesAndSkus);

            foreach (var activity in activities)
            {
                foreach(var sku in activity.Skus)
                {
                   sku.PriceOptions =  BuildPriceOptions();
                }
                activity.Equiptment = equiptment;
            }
            return activities;
        }

        private static IList<ActivitySkuPrice> BuildPriceOptions()
        {
            return new[]
            {
                new ActivitySkuPrice()
                {
                    DescriptionContent = new[]
                    {
                        new Content()
                        {
                            ContentType = "Text",
                            Value = "Join a group",
                            Locale = "na",
                            Id = "GroupDesc"
                        }
                    },
                    MaxPersons = 8,
                    MinPersons = 1,
                    Name = "Group",
                    Price = 210 * 4
                },
                    new ActivitySkuPrice()
                {
                    DescriptionContent = new[]
                    {
                        new Content()
                        {
                            ContentType = "Text",
                            Value = "Private tour",
                            Locale = "na",
                            Id = "PrivateTourDesc"
                        }
                    },
                    MaxPersons = 2,
                    MinPersons = 1,
                    Name = "Private2",
                    Price = 660
                },
                    new ActivitySkuPrice()
                {
                    DescriptionContent = new[]
                    {
                        new Content()
                        {
                            ContentType = "Text",
                            Value ="Private tour",
                            Locale = "na",
                            Id = "PrivateTourDesc"
                        }
                    },
                    MaxPersons = 3,
                    MinPersons = 3,
                    Name = "Private3",
                    Price = 810
                },
                    new ActivitySkuPrice()
                {
                    DescriptionContent = new[]
                    {
                        new Content()
                        {
                            ContentType = "Text",
                            Value ="Private tour",
                            Locale = "na",
                            Id = "PrivateTourDesc"
                        }
                    },
                    MaxPersons = 4,
                    MinPersons = 4,
                    Name = "Private4",
                    Price = 840
                }
            };
        }

        protected static ActivityEquiptment[] BuildEquiptment()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityEquiptment[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.Equiptment);
        }

        protected static ActivitySku[] BuildActivitySkus()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ActivitySku[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.ActivitySkus);
        }
        protected static Activity[] BuildActivities()
        {
            var activities = Newtonsoft.Json.JsonConvert.DeserializeObject<Activity[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.Activities);
            var equiptment = BuildEquiptment();

            foreach (var activity in activities)
            {
                activity.Equiptment = equiptment;
            }
            return activities;
        }
    }
}
