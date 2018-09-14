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
    public class ActivityTestFixture : BaseTestFixture
    {
       

        public ActivityTestFixture(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task AddAndUpdateActivitySku()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");

            string name = "Allalin4028_" + new Random().Next(1000);
            await AddActivity(authClient, name);

            await authClient.AddActivitySku(BuildActivitySku(name));
            await authClient.UpdateActivitySku(BuildActivitySku(name));


        }

        private static ActivitySku BuildActivitySku(string name)
        {
            return new ActivitySku()
            {
                Name = name,
                ActivityName = name,
                DescriptionContent =
                            new Content[]
                            {
                    new Content
                    {
                        ContentType = "HTML",
                        Locale = "en",
                        Id = "Allalin4027SkuDesc",
                        Value = @"<b>MY FIRST 4000M PEAK!</b>
The Allalin is known as one of the easier 4000m peaks in the Alps and offers an unforgetable experience for good hikers which aim higher!



Some regard the Allalinhorn as a training tour, others take it on for acclimatisation and for quite a few others it is a real holiday highlight and possibly the only 4000er they will ever climb.Whatever your goal, it will be an unforgettable experience."
                    }
                            },
                TitleContent = new Content[]
                            {
                    new Content
                    {
                        ContentType = "Text",
                        Locale = "en",
                        Id = "Allalin4027SkuTitle",
                        Value = "ALLALINHORN 4027M"
                    }
                            },
                AdditionalRequirementsContent = new Content[]
                            {
                    new Content
                    {
                        ContentType = "HTML",
                        Locale = "en",
                        Id = "Allalin4027AddRequirements",
                        Value = @"+  Easy glacier tour. Minimal gradient.

+  Good basic fitness level required."
                    }
                            },
                DurationHours = 4,
                MaxPersons = 8,
                MinPersons = 1,
                PricePerPerson = 210,
                WebContent = new Content[]
                            {
                    new Content
                    {
                        ContentType = "HTML",
                        Locale = "en",
                        Id =  "Allalin4027WebContent",
                        Value = @"test"
                    }
                            },

            };
        }

        [Fact]
        public async Task AddAndUpdateActivity()
        {
            var authClient = await AuthenticatedClient("testadmin", "password", "test.admin@sfg.ch");
            string name = "Allalin4027_" + new Random().Next(1000);
            await AddActivity(authClient, name);

            await authClient.UpdateActivity(BuildActivity(name));
        }

        private static async Task AddActivity(AuthenticatedClient authClient, string name)
        {
            await authClient.AddActivity(BuildActivity(name));          
        }

        private static Activity BuildActivity(string name)
        {
            return new Activity()
            {
               
                Name = name,
                DescriptionContent =
                            new Content[]
                            {
                    new Content
                    {
                        ContentType = "text",
                        Locale = "en",
                        Id =   "Allalin4027Desc",
                        Value = "My first 4000m peak! The Allalin is known as one of the easier 4000m peaks in the Alps and offers an unforgetable..."
                    }
                            },
                TitleContent = new Content[]
                            {
                    new Content
                    {
                        ContentType = "Text",
                        Locale = "en",
                        Id = name + "Allalin4027Title",
                        Value = "ALLALINHORN 4027M"
                    }
                            },
                MenuImageContentId = new Content
                {
                    Id = "Allalin4027MenuImage",
                    ContentType = "Image",
                    Value = name + ".jpg",
                    Locale = "na"
                }
            };
        }
    }
}
