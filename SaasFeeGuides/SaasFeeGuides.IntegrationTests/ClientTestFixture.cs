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
        private async Task<AuthenticatedClient> AuthenticatedClient(string username,string password,string email)
        {
            if (_authenticatedClient == null)
            {
                var loginResponse = await Login(username, password, email);
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
            var authClient = await AuthenticatedClient("test","password","test.user@sfg.ch");
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
        public async Task AddAdminAccount()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");
            
                await authClient.AddClaim(new AppClaim()
                {
                    ClaimType = "role",
                    ClaimValue = "api_access"
                });
           

        }

        [Fact]
        public async Task AddClaim()
        {
            var authClient = await AuthenticatedClient("test", "password", "test.user@sfg.ch");
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
                    DateOfBirth = new DateTime(1984,10,31),
                    
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
