using Autofac.Extensions.DependencyInjection;
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
      return WebHost.CreateDefaultBuilder(args)
        .UseKestrel(options => { options.Limits.MaxRequestBodySize = null; })
        .ConfigureAppConfiguration((context, builder) =>
        {
          var env = context.HostingEnvironment;
          builder.AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
          builder.AddEnvironmentVariables();
        })
        .ConfigureServices(service => service.AddAutofac())
        .ConfigureLogging((context, builder) =>
        {
          builder.AddConfiguration(context.Configuration.GetSection("Logging"));
          builder.AddConsole();
          builder.AddDebug();
        })
        .UseStartup<Startup>()
        .Build();
    }
  }
}
