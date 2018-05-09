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
      var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
      containerBuilder.RegisterInstance(new TelegramBotClient(botToken)).As<ITelegramBotClient>();

      containerBuilder.Populate(services);

      //services.AddScoped<IUpdateHub, UpdateHub>();
      

      //services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider=>
      //  new TelegramBotClient(botToken, new HttpClient()
      //  {
      //    BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url"))
      //  }));
      //containerBuilder.RegisterType<Dictionary<long, ChatProperties>>().SingleInstance();
      //containerBuilder.RegisterType<DefaultChatService>().As<IDefaultChatService>();
      //containerBuilder.RegisterType<InitChatService>().As<IInitChatService>();
      //containerBuilder.RegisterType<StartChatService>().As<IStartChatService>();
      var container = containerBuilder.Build();
      services.AddSingleton<ContextHub>(provider => new ContextHub(container));
      //services.AddSingleton<IContainer>(containerBuilder.Build());
      //services.AddSingleton(Configuration);

      services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
      return new AutofacServiceProvider(container);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
