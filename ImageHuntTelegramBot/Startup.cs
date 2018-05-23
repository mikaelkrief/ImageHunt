using System;
using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImageHuntTelegramBot.Controllers;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ImageHuntTelegramBot
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      // Add Autofac as dependency injection
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterModule<DefaultModule>();
      containerBuilder.RegisterInstance(Configuration);
      containerBuilder.RegisterInstance(new HttpClient()
      {
        BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url"))
      });
      containerBuilder.RegisterInstance(
        new FileStorage(Configuration.GetValue<string>("BotConfiguration:StorageFolder"))).As<IStorage>();
      var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
      containerBuilder.RegisterInstance(new TelegramBotClient(botToken)).As<ITelegramBotClient>();

      containerBuilder.Populate(services);

      var container = containerBuilder.Build();

      services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
      return new AutofacServiceProvider(container);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
