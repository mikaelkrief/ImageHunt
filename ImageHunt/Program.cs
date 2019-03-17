using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

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
          return WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            {
              options.Limits.MaxRequestBodySize = null;
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
              var env = context.HostingEnvironment;
              builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
              builder.AddEnvironmentVariables();
            })
            .UseSerilog((context, configuration) => configuration
              .ReadFrom
              .Configuration(context.Configuration))
            .ConfigureServices(service=>service.AddAutofac())
             .UseStartup<Startup>()
             
           .Build();
        
        }
    }
}
