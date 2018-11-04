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
            var customer = await authClient.GetCustomer(customers[0].Id.Value);

            Assert.Equal(customer.FirstName, customers[0].FirstName);
            Assert.Equal(3, customers.Count);
        }
        [Fact]
        public async Task GetCustomers_Search()
        {
            var authClient = await AuthClient();
            await AddCustomersIfNeeded(authClient);
            var customers = await authClient.GetCustomers("james");
            var customer = await authClient.GetCustomer(customers[0].Id.Value);

            Assert.Equal(customer.FirstName, customers[0].FirstName);
            Assert.Equal(1, customers.Count);
        }
        [Fact]
        public async Task AddCustomer()
        {
            var authClient = await AuthClient();
            var customerIds = await AddCustomersIfNeeded(authClient);
            var customers = await authClient.GetCustomers();
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public async Task AddCustomerBooking()
        {
            var authClient = await AuthClient();

            await AddActivitiesAndSkusIfNeeded(authClient);
            var customers = await authClient.GetCustomers();
            await AddCustomerBookingsIfNeeded(authClient);

        }

        [Fact]
        public async Task UpdateCustomerBooking()
        {
            var authClient = await AuthClient();

            await AddActivitiesAndSkusIfNeeded(authClient);
            var customers = await authClient.GetCustomers();
            await AddCustomerBookingsIfNeeded(authClient);

            var dates = await authClient.GetAllActivityDates(null, null);
            var date = dates.FirstOrDefault(x=>x.CustomerBookings?.Any() ?? false);
            var booking = date.CustomerBookings[0];
            booking.HasPaid = !booking.HasPaid;
            booking.HasConfirmed = !booking.HasConfirmed;
            booking.CustomerNotes = "1 leg";

            await authClient.UpdateCustomerBooking(booking);

            dates = await authClient.GetAllActivityDates(null, null);
            date = dates.FirstOrDefault(x => x.CustomerBookings?.Any() ?? false);
            booking = date.CustomerBookings[0];

            Assert.True(booking.HasPaid);
            Assert.True(booking.HasConfirmed);
            Assert.Equal("1 leg", booking.CustomerNotes);
        }

        [Fact]
        public async Task AddHistoricCustomerBooking()
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
               ActivitySkuName = "Allalin",
               AmountPaid = 210,
               DateTime = new DateTime(2017,12,31,9,0,0),
               NumPersons = 1
            });

            Assert.True(customerId > 0);
            Assert.True(customerBookingId.customerBookingId > 0);
        }
    }
}
