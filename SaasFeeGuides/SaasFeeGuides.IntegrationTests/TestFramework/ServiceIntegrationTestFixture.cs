using Microsoft.AspNetCore.Hosting;
using SaasFeeGuides.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.IntegrationTests.TestFramework
{
    public class ServiceIntegrationTestFixture : IDisposable
    {
        //private readonly ITestOutputHelper _output;

        //public ServiceIntegrationTestFixture(ITestOutputHelper output)
        //{
        //    _output = output;
        //}

        public IWebHost Webhost { get; private set; }

        public void StartServiceForTest<TStartup>(string[] urls, string workingDirectory = null)
            where TStartup : class
        {
            try
            {
                Webhost = ProgramHelper.BuildHost<TStartup>(urls, workingDirectory);

                //_output.WriteLine($"Webhost Starting {typeof(TStartup)}");
                Webhost.StartAsync().Wait();

            }
            catch (Exception ex)
            {
                Webhost?.StopAsync().Wait();
                throw;
            }
        }

        public void Dispose()
        {
            //_output.WriteLine($"Webhost Stopping (Dispose())");
            Webhost?.StopAsync().Wait();
        }
    }
}
