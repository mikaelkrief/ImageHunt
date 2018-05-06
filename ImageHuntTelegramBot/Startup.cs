using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Autofac;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntTelegramBot.Services;
using ImageHuntTelegramBot.WebServices;
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

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddScoped<IUpdateHub, UpdateHub>();
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterType<Dictionary<long, ChatProperties>>().SingleInstance();
      containerBuilder.RegisterType<DefaultChatService>().As<IDefaultChatService>();
      containerBuilder.RegisterType<InitChatService>().As<IInitChatService>();
      containerBuilder.RegisterType<StartChatService>().As<IStartChatService>();
      containerBuilder.RegisterType<GameWebService>().As<IGameWebService>();
      containerBuilder.RegisterType<TeamWebService>().As<ITeamWebService>();
      containerBuilder.RegisterInstance(new HttpClient()
      {
        BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url"))
      });
      var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
      containerBuilder.RegisterInstance(new TelegramBotClient(botToken)).As<ITelegramBotClient>();
      services.AddSingleton<IContainer>(containerBuilder.Build());
      services.AddSingleton(Configuration);

      services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
