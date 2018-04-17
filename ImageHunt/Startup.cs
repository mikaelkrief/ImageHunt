using System;
using System.IO;
using System.Net.Http;
using AutoMapper;
using ImageHunt.Controllers;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Request;
using ImageHunt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;

namespace ImageHunt
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc()
        .AddJsonOptions(options=>options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
      services.AddAuthorization();
      services.AddTransient<IAuthorizationHandler, TokenAuthorizationHandler>();
      services.AddDbContext<HuntContext>(options =>
          options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
      services.AddSingleton(Configuration);
      services.AddTransient(s =>
      {
        var sb = services.BuildServiceProvider();
        return new AuthControllerParameters(Configuration,
          new HttpClient() { BaseAddress = new Uri(Configuration["GoogleApi:AccessTokenUrl"]) },
          new HttpClient() { BaseAddress = new Uri(Configuration["GoogleApi:UserInfoUrl"]) },
          sb.GetService<IAuthService>()
        );
      });
      services.AddTransient<ITeamService, TeamService>();
      services.AddTransient<IAdminService, AdminService>();
      services.AddTransient<IGameService, GameService>();
      services.AddTransient<IAuthService, AuthService>();
      services.AddTransient<IImageService, ImageService>();
      services.AddTransient<INodeService, NodeService>();
      services.AddTransient<IPlayerService, PlayerService>();
      services.AddTransient<IActionService, ActionService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, HuntContext dbContext)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      dbContext.Database.Migrate();
      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
            !Path.HasExtension(context.Request.Path.Value) &&
            !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      app.UseMvcWithDefaultRoute();
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseMvc();
      ConfigureMappings();
    }

    public static void ConfigureMappings()
    {
      Mapper.Initialize(config =>
      {
        config.CreateMap<AddNodeRequest, Node>().ConstructUsing(r=>NodeFactory.CreateNode(r.NodeType));
        config.CreateMap<GameActionRequest, GameAction>().ForMember(x => x.Picture, opt => opt.Ignore());

      });
    }
  }
}
