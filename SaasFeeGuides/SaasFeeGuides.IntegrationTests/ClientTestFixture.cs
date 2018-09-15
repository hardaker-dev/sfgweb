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
    [Collection("Database collection")]
    public class ClientTestFixture : BaseTestFixture
    {
       

        public ClientTestFixture(ITestOutputHelper output)
            : base(output)
        {




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

       
    }
}
