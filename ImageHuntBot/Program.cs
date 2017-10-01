using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ImageHuntBot
{
    public class Program
    {
    
        public static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
        .AddCommandLine(args)
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables();
          var configuration = configurationBuilder.Build();
          configurationBuilder.AddJsonFile($"appsettings.{configuration["ASPNETCORE_ENVIRONMENT"]}.json");
          configuration = configurationBuilder.Build();
          var telegramClient = new TelegramBotClient(configuration["Telegram:APIKey"]);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
