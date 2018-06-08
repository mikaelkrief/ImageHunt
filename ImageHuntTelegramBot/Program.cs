using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEscapades.Extensions.Logging.RollingFile;

namespace ImageHuntTelegramBot
{
    class Program
    {
      public static void Main(string[] args)
      {
        BuildWebHost(args).Run();
      }

      public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment;
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                builder.AddEnvironmentVariables();
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.AddConfiguration(context.Configuration.GetSection("Logging"));
              builder.AddConsole();
              builder.AddDebug();
              builder.AddFile(options =>
              {
                options.LogDirectory = context.Configuration["Logging:LogDirectory"];
                options.FileName = "ImageHuntBot";
              });
            })

          .UseStartup<Startup>()
          .Build();
    
  }
}
