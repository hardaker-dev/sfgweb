using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Helpers
{
    public class ProgramHelper
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="urls"></param>
     /// <param name="workingDirectory"></param>
     /// <returns></returns>
        public static IWebHost BuildHost<TStartup>(string[] urls = null, string workingDirectory = null)
            where TStartup : class
        {
            workingDirectory = workingDirectory ?? Directory.GetCurrentDirectory();

            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(workingDirectory)
                
                .UseIISIntegration()
                .UseStartup<TStartup>();

            if (urls?.Any() ?? false)
            {
                hostBuilder.UseUrls(urls);
            }

            var host = hostBuilder.Build();
            return host;
        }
    }
}
