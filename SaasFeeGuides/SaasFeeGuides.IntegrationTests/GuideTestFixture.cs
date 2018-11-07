using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SaasFeeGuides.IntegrationTests.TestFramework;
using SaasFeeGuides.RestClient;
using SaasFeeGuides.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests
{
    [Collection("Database collection")]
    public class GuideTestFixture : BaseTestFixture
    {
       

        public GuideTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task GetGuides()
        {
            var authClient = await AuthClient();
            await AddGuidesIfNeeded(authClient);
            var guides =  await authClient.GetGuides();
            var guide = await authClient.GetGuide(guides[0].Id.Value);

            Assert.Equal(guide.FirstName, guides[0].FirstName);
            Assert.Equal(2, guides.Count);
        }
        [Fact]
        public async Task GetGuides_Search()
        {
            var authClient = await AuthClient();
            await AddGuidesIfNeeded(authClient);
            var guides = await authClient.GetGuides("paul");
            var guide = await authClient.GetGuide(guides[0].Id.Value);

            Assert.Equal(guide.FirstName, guides[0].FirstName);
            Assert.Equal(1, guides.Count);
        }
        [Fact]
        public async Task AddGuide()
        {
            var authClient = await AuthClient();
            var guideIds = await AddGuidesIfNeeded(authClient);
            var guides = await authClient.GetGuides();
            Assert.Equal(2, guides.Count);
        }


    

       
    }
}
