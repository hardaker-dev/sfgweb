using SaasFeeGuides.IntegrationTests.TestFramework;
using SaasFeeGuides.RestClient;
using SaasFeeGuides.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests
{
    public class BaseTestFixture : ServiceIntegrationTestBase<TestStartup>
    {
        protected readonly Client _client;
        private AuthenticatedClient _authenticatedClient;
        protected async Task<AuthenticatedClient> AuthenticatedClient(string username, string password, string email)
        {
            if (_authenticatedClient == null)
            {
                var loginResponse = await Login(username, password, email);
                _authenticatedClient = new AuthenticatedClient(ServiceUri, loginResponse.auth_token);
            }
            return _authenticatedClient;
        }

        public BaseTestFixture(ITestOutputHelper output)
            : base(output, 5000, 6000)
        {
            this._client = new Client(ServiceUri);




        }

        private async Task<LoginResponse> Login(string username, string password, string email)
        {
            try
            {
                await _client.AddAccount(new ViewModels.Registration()
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
    }
}
