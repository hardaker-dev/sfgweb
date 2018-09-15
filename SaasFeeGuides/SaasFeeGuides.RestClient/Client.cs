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
    public class Client
    {
        protected readonly Uri _serviceUri;

        public Client(Uri serviceUri)
        {
            _serviceUri = serviceUri;
        }

        protected HttpClient GetHttpClient()
        {
            return DefaultClient;
        }

        public static HttpClient DefaultClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip
        });
        public async Task<IList<DateTime>> GetActivitySkuDates(int activitySkuId, DateTime? dateFrom,DateTime? dateTo)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity","sku", activitySkuId,"date")
                  .WithQueryParameters(new Dictionary<string, object>()
                {
                    {"dateFrom",dateFrom?.ToString(ApiConstants.DateStringFormat) },
                    {"dateTo",dateTo?.ToString(ApiConstants.DateStringFormat) }
                })
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<IList<DateTime>>();
        }
        public async Task<ActivitySkuLoc> GetActivitySku(int activitySkuId, string locale)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity","sku", activitySkuId)
                  .WithQueryParameters(new Dictionary<string, object>()
                {
                    {"locale",locale }
                })
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<ActivitySkuLoc>();
        }
        public async Task<ActivityLoc> GetActivity(int activityId,string locale)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity",activityId)
                  .WithQueryParameters(new Dictionary<string, object>()
                {
                    {"locale",locale }
                })
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<ActivityLoc>();
        }
        public async Task<IList<ActivityLoc>> GetActivities(string locale)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "activity")
                  .WithQueryParameters(new Dictionary<string, object>()
                {
                    {"locale",locale }
                })
                .AcceptGzipCompression();

            var get = request.GetAsync(DefaultClient);
            var response = await get;

            return await get.ReceiveJsonAsync<IList<ActivityLoc>>();
        }
        public async Task AddAccount(Registration registrationViewModel)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "account")
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(registrationViewModel, DefaultClient);
            var response = await post;

            if (!response.IsSuccessStatusCode)
            {
                throw new RestResponseException(response.StatusCode, response.RequestMessage, string.Empty, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<LoginResponse> Login(Credentials credentialsViewModel)
        {
            var request = _serviceUri.AsRestRequest()
                .WithPathSegments("api", "auth","login")
                .AcceptGzipCompression();

            var post = request.PostJsonAsync(credentialsViewModel, DefaultClient);
            var response = await post;

            return await post.ReceiveJsonAsync<LoginResponse>();

        }   
      
    }
}
