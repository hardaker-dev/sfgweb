using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SaasFeeGuides.IntegrationTests.TestFramework
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override bool IsDevelopment(IHostingEnvironment env)
        {
            return true;
        }
    }
}
