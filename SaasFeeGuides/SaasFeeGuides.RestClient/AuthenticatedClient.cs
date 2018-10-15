using Newtonsoft.Json;
using RiskFirst.RestClient;
using SaasFeeGuides.RestClient.Extensions;
using SaasFeeGuides.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SaasFeeGuides.RestClient
{
    public class AuthenticatedClient : Client
    {

        private readonly string _bearerToken;

        public AuthenticatedClient(Uri serviceUri,string bearerToken) : base(serviceUri)
        {
            _bearerToken = bearerToken;
        }

        public async Task<int> AddActivitySkuDate(ActivitySkuDate activitySkuDate)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity", "sku","date")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(activitySkuDate, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<int>();

        }

        public async Task<Activity> GetActivity(int activityId)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity", activityId, "edit")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<Activity>();
        }

        public async Task<IList<Activity>> GetActivities()
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity", "edit")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<IList<Activity>>();
        }

        public async Task<int> AddOrUpdateEquiptment(Equiptment equiptment)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "equiptment")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PutJsonAsync(equiptment, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<int>();

        }
        public async Task<int> AddOrUpdateActivitySku(ActivitySku activitySku)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity","sku")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PutJsonAsync(activitySku, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<int>();

        }

        public async Task<(int customerBookingId, int activitySkuDateId)> AddHistoricCustomerBooking(HistoricCustomerBooking booking)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer","booking","historic")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(booking, DefaultClient);
            var response = await post;

            return await post.ReceiveJsonAsync<(int,int)>();
        }

        public async Task<IEnumerable<CustomerBooking>> GetCustomerBookings(int customerId)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer",customerId, "booking")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync( DefaultClient);
            var response = await get;
            return await get.ReceiveJsonAsync<IEnumerable<CustomerBooking>>();
        }

        public async Task<(int customerBookingId,int activitySkuDateId)> AddCustomerBooking(CustomerBooking customer)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer","booking")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(customer, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<(int customerBookingId, int activitySkuDateId)>();

        }
        public async Task<int> AddCustomer(Customer customer)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(customer, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<int>();

        }
        public async Task<int> AddActivity(Activity activity)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(activity, DefaultClient);
            var response = await post;


            return await post.ReceiveJsonAsync<int>();

        }
        public async Task<Customer> GetCustomer(int customerId)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer", customerId)
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;


            return await get.ReceiveJsonAsync<Customer>();

        }
        public async Task<IList<Customer>> GetCustomers(string searchText=null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (searchText != null)
            {
                parameters.Add("searchText", searchText);
            }
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "customer")
                .WithQueryParameters(parameters)
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync( DefaultClient);
            var response = await get;


            return await get.ReceiveJsonAsync<IList<Customer>>();

        }
        public async Task UpdateActivity(Activity activity)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var patch = request.PatchJsonAsync(activity, DefaultClient);
            var response = await patch;

            if (!response.IsSuccessStatusCode)
            {
                throw new RestResponseException(response.StatusCode, response.RequestMessage, string.Empty, await response.Content.ReadAsStringAsync());
            }

        }

        public async Task AddClaim(AppClaim claim)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "claim")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(claim,DefaultClient);
            var response = await post;

            if (!response.IsSuccessStatusCode)
            {
                throw new RestResponseException(response.StatusCode, response.RequestMessage, string.Empty, await response.Content.ReadAsStringAsync());
            }

        }

        public async Task<DashboardIndex> GetDashboardIndex()
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "dashboard", "index")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<DashboardIndex>();

        }
        public new async Task<IList<ActivityDateAdmin>> GetActivityDates(int activityId, DateTime? dateFrom, DateTime? dateTo)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity", activityId, "dates")
                  .WithBearerToken(_bearerToken)
                  .WithQueryParameters(new Dictionary<string, object>()
                {
                    {"dateFrom",dateFrom?.ToString(ApiConstants.DateStringFormat) },
                    {"dateTo",dateTo?.ToString(ApiConstants.DateStringFormat) }
                })
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<IList<ActivityDateAdmin>>();
        }
        public async Task DeleteActivitySkuDate(int activitySkuDateId)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity","sku","date",activitySkuDateId)
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

            var post = request.DeleteAsync(DefaultClient);
            var response = await post;

            if (!response.IsSuccessStatusCode)
            {
                throw new RestResponseException(response.StatusCode, response.RequestMessage, string.Empty, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteAccount()
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "account")
                .WithBearerToken(_bearerToken)
                .AcceptGzipCompression();

              var post = request.DeleteAsync( DefaultClient);
              var response = await post;

            if (!response.IsSuccessStatusCode)
            {
                throw new RestResponseException(response.StatusCode, response.RequestMessage, string.Empty, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
