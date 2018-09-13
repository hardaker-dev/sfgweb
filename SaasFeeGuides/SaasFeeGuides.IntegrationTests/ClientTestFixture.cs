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
    public class ClientTestFixture : ServiceIntegrationTestBase<TestStartup>
    {
        private readonly Client _client;
        private AuthenticatedClient _authenticatedClient;
        private async Task<AuthenticatedClient> AuthenticatedClient()
        {
            if (_authenticatedClient == null)
            {
                var loginResponse = await Login();
                _authenticatedClient = new AuthenticatedClient(ServiceUri, loginResponse.auth_token);
            }
            return _authenticatedClient;
        }

        public ClientTestFixture(ITestOutputHelper output)
            : base(output,5000, 6000)
        {
            this._client = new Client(ServiceUri);




        }
        [Fact]
        public async Task GetDashboardIndex()
        {
            var authClient = await AuthenticatedClient();
            try
            {
                await authClient.AddClaim(new AppClaim()
                {
                    ClaimType = "role",
                    ClaimValue = "api_access"
                });
                await authClient.GetDashboardIndex();
            }
            finally
            {
                await authClient.DeleteAccount();
            }
        }
        [Fact]
        public async Task AddClaim()
        {
            var authClient = await AuthenticatedClient();
            try
            {
                await authClient.AddClaim(new AppClaim()
                {
                    ClaimType = "role",
                    ClaimValue = "api_access"
                });
            }
            finally
            {
                await authClient.DeleteAccount();
            }

        }

        private async Task<LoginResponse> Login()
        {
            try
            {
                await _client.AddAccount(new ViewModels.Registration()
                {
                    Email = "test.1@testemail.com",
                    FirstName = "james",
                    LastName = "hardaker",
                    Password = "password",
                    Username = "JamesHardaker",
                    IsAdmin = true,
                    DateOfBirth = new DateTime(1984,10,31),
                    
                });
            }
            catch (Exception e)
            {

            }
            var loginResponse = await _client.Login(new ViewModels.Credentials()
            {
                UserName = "JamesHardaker",
                Password = "password"
            });
            return loginResponse;
        }
    }
}
