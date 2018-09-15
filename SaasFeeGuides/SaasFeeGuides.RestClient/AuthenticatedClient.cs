using Newtonsoft.Json;
using RiskFirst.RestClient;
using SaasFeeGuides.RestClient.Extensions;
using SaasFeeGuides.ViewModels;
using System;
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
