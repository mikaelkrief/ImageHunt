using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImageHunt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) 
        {
          var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
          return WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(configuration)
            .UseStartup<Startup>()
            .ConfigureLogging((context, builder) =>
              {
                builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
              })
            .Build();
        
        }
    }
}
