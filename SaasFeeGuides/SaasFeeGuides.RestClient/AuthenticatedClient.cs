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
