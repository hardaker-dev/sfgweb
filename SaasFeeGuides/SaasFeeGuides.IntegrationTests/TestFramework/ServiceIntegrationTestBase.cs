using Microsoft.Extensions.Configuration;
using SaasFeeGuides.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SaasFeeGuides.IntegrationTests.TestFramework
{
    [Collection("ServiceIntegrationTests_NonParallel")]
    public abstract class ServiceIntegrationTestBase<TStartup> : IDisposable
       where TStartup : class
    {
        protected ITestOutputHelper Output { get; }

        public ServiceIntegrationTestFixture Fixture { get; }

        public Uri ServiceUri { get; }

        

        protected ServiceIntegrationTestBase(ITestOutputHelper output, int minPort, int maxPort)
        {
#if AZURETEST
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "AzureTest");
#endif

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env}.json", optional: true)
              .Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];

         
            var serviceEndpoint = config["serviceEndpoint"];

            SaasFeeGuides.Database.Deploy.Program.DeployDatabase(connectionString);
            Output = output;
            if (string.IsNullOrEmpty(serviceEndpoint))
            {
                var url =  TcpSocketHelper.GetNextLocalhostUrl(minPort, maxPort, null);
                Output.WriteLine($"{this.GetType()} StartServiceForTest<{typeof(TStartup)} on {url}");
                ServiceUri = new Uri(url);
                Fixture = new ServiceIntegrationTestFixture();
                Fixture.StartServiceForTest<TStartup>(new[] { url });
            }
            else
            {
                ServiceUri = new Uri(serviceEndpoint);
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Fixture?.Dispose();
        }

        #endregion
    }
}
