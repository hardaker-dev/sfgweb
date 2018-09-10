using SaasFeeGuides.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
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
            Output = output;
            var url = TcpSocketHelper.GetNextLocalhostUrl(minPort, maxPort, null);
            Output.WriteLine($"{this.GetType()} StartServiceForTest<{typeof(TStartup)} on {url}");
            ServiceUri = new Uri(url);
            Fixture = new ServiceIntegrationTestFixture();
            Fixture.StartServiceForTest<TStartup>(new[] { url });
        }

        #region IDisposable

        public void Dispose()
        {
            Fixture?.Dispose();
        }

        #endregion
    }
}
