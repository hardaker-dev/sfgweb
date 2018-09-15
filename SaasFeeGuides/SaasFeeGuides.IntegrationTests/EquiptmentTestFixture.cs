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
    public class EquiptmentTestFixture : BaseTestFixture
    {
       

        public EquiptmentTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }
      
        [Fact]
        public async Task AddOrUpdateEquiptment()
        {
            var authClient = await AuthClient();
            var equiptments = Newtonsoft.Json.JsonConvert.DeserializeObject<Equiptment[]>(SaasFeeGuides.IntegrationTests.Properties.PostTestData.Equiptment);

            foreach (var equiptment in equiptments)
            {
                var equiptmentId = await authClient.AddOrUpdateEquiptment(equiptment);

                Assert.True(equiptmentId > 0);
            }
        }

//        climbing harness
//ice axe
//crampons with antibott
//hiking pole

    }
}
