using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace SaasFeeGuides.Database.Deploy
{
    public class Program
    {
        static int Main(string[] args)
        {
            //        ?? "Server=tcp:testsfgsql01.database.windows.net,1433;Initial Catalog=sfg;Persist Security Info=False;User ID=JamesHardaker;Password=Trek4ever!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env}.json", optional: true)
              .Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];
      
          //  var connectionString =
                
           //     ?? "Server=tcp:testsfgsql01.database.windows.net,1433;Initial Catalog=sfg;Persist Security Info=False;User ID=JamesHardaker;Password=Trek4ever!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return DeployDatabase(connectionString);
        }

        public static void DeleteDatabase(string connectionString)
        {
            DropDatabase.For.SqlDatabase(connectionString);
        }

        public static int DeployDatabase(string connectionString)
        {
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
