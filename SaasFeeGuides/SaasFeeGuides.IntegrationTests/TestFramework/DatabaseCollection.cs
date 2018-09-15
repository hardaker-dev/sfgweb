using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SaasFeeGuides.IntegrationTests.TestFramework
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
#if AZURETEST
            var env = "AzureTest";
#else
            var env = "Development";
            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env}.json", optional: true)
             .Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];

            SaasFeeGuides.Database.Deploy.Program.DeleteDatabase(connectionString);
#endif
            // ... initialize data in the test database ...
        }

        public void Dispose()
        {



            // ... clean up test data from the database ...
        }

    }
}
