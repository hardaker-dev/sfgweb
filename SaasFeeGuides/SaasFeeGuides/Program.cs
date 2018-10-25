using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SaasFeeGuides.Helpers;

namespace SaasFeeGuides
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            Startup.CommandLineArguments = args;
            ProgramHelper.BuildHost<Startup>().Run();
            // CreateWebHostBuilder(args).Build().Run();

            
        }

    }
}
