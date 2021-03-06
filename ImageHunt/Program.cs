using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
              builder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.webapi.json", optional:true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
              builder.AddEnvironmentVariables();
            })
            .UseApplicationInsights("6d5d005b-8892-4721-8d20-fd03ceec3548")
            .ConfigureServices(service=>service.AddAutofac())
             .UseStartup<Startup>()
             
           .Build();
        
        }
    }
}
