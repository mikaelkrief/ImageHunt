using System;
using System.Net.Http;
using Autofac;
using ImageHuntTelegramBot.Controllers;
using ImageHuntTelegramBot.Dialogs;
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

      //services.AddScoped<IUpdateHub, UpdateHub>();
      var containerBuilder = new ContainerBuilder();
      var telegramBot = new TelegramBot();
      telegramBot.AddDialog("/init", new InitDialog());
      services.AddSingleton<IBot, TelegramBot>(provider => telegramBot);
      var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
      
      //services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider=>
      //  new TelegramBotClient(botToken, new HttpClient()
      //  {
      //    BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url"))
      //  }));
      //containerBuilder.RegisterType<Dictionary<long, ChatProperties>>().SingleInstance();
      //containerBuilder.RegisterType<DefaultChatService>().As<IDefaultChatService>();
      //containerBuilder.RegisterType<InitChatService>().As<IInitChatService>();
      //containerBuilder.RegisterType<StartChatService>().As<IStartChatService>();
      //containerBuilder.RegisterType<GameWebService>().As<IGameWebService>();
      //containerBuilder.RegisterType<TeamWebService>().As<ITeamWebService>();
      containerBuilder.RegisterInstance(new HttpClient()
      {
        BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url"))
      });
      containerBuilder.RegisterInstance(new TelegramBotClient(botToken)).As<ITelegramBotClient>();
      containerBuilder.RegisterType<TelegramAdapter>().As<IAdapter>();
      containerBuilder.RegisterType<TurnContext>().As<ITurnContext>();
      var container = containerBuilder.Build();
      services.AddSingleton<ContextHub>(provider => new ContextHub(container));
      //services.AddSingleton<IContainer>(containerBuilder.Build());
      //services.AddSingleton(Configuration);

      services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
