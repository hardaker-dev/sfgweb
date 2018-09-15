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
    public class AccountTestFixture : BaseTestFixture
    {
       

        public AccountTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }
        [Fact]
        public async Task GetDashboardIndex()
        {
            var authClient = await AuthClient();
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
            var authClient = await AuthClient();

            await authClient.AddClaim(new AppClaim()
                {
                    ClaimType = "role",
                    ClaimValue = "api_access"
                });
           

        }

        [Fact]
        public async Task AddClaim()
        {
            var authClient = await AuthClient();
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
