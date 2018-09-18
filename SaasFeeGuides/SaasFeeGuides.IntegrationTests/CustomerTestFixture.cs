using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SaasFeeGuides.IntegrationTests.TestFramework;
using SaasFeeGuides.RestClient;
using SaasFeeGuides.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests
{
    [Collection("Database collection")]
    public class CustomerTestFixture : BaseTestFixture
    {
       

        public CustomerTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task GetCustomers()
        {
            var authClient = await AuthClient();
            await AddCustomersIfNeeded(authClient);
            var customers =  await authClient.GetCustomers();

            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public async Task AddCustomer()
        {
            var authClient = await AuthClient();
            var customerIds = await AddCustomersIfNeeded(authClient);

            Assert.Equal(3,customerIds.Count);
        }

        private static async Task<IList<int>> AddCustomersIfNeeded(AuthenticatedClient authClient)
        {
            var customers = await authClient.GetCustomers();
            var ids = customers.Select(c => c.Id.Value).ToList();
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

        [Fact]
        public async Task AddCustomerBooking()
        {
            var authClient = await AuthClient();

            await AddActivitiesAndSkusIfNeeded(authClient);
            var customers = await authClient.GetCustomers();
            var customerId = customers.FirstOrDefault(c=> c.Email == "dude@gmail.com")?.Id ??  await authClient.AddCustomer(new Customer()
            {
                Address = "London",
                DateOfBirth = new DateTime(1984, 10, 31),
                Email = "dude@gmail.com",
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "+447567123456"
            });
            var customerBookingId = await authClient.AddHistoricCustomerBooking(new HistoricCustomerBooking()
            {
               CustomerEmail = "dude@gmail.com",
               ActivitySkuName = "AllalinSku",
               AmountPaid = 210,
               Date = new DateTime(2017,12,31),
               NumPersons = 1
            });

            Assert.True(customerId > 0);
            Assert.True(customerBookingId > 0);
        }
    }
}
