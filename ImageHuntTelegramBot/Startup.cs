using System;
using System.Collections.Generic;
using System.Text;
using ImageHuntTelegramBot.Services;
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

      services.AddScoped<IUpdateService, UpdateService>();
      services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
