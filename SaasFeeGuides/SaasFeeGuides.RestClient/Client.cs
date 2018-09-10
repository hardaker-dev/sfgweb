﻿using Newtonsoft.Json;
using RiskFirst.RestClient;
using SaasFeeGuides.RestClient.Extensions;
using SaasFeeGuides.ViewModels;
using System;
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
