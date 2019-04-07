using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using ImageHunt.Computation;
using ImageHunt.Controllers;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHunt.Updater;
using ImageHuntCore;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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
      #region ===== Add Identity ========
      services.AddIdentity<Identity, IdentityRole>()
        .AddEntityFrameworkStores<HuntContext>()
        .AddDefaultTokenProviders();
      #endregion
      #region ===== Add Jwt Authentication ========
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
      services
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(cfg =>
        {
          cfg.RequireHttpsMetadata = false;
          cfg.SaveToken = true;
          cfg.TokenValidationParameters = new TokenValidationParameters
          {
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
          };
        });
      #endregion

      services.AddAuthorization(options =>
      {
        options.AddPolicy("ApiUser",
          policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol,
            Constants.Strings.JwtClaims.ApiAccess));
      });
      //services.AddCors();
      services.AddMvc()
        .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
      var dbContextBuilder = new DbContextOptionsBuilder<HuntContext>().UseMySql(Configuration.GetConnectionString("DefaultConnection"));
      services.AddTransient(s =>
        ActivableContext<HuntContext>.CreateInstance(dbContextBuilder.Options));
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
      ConfigureMappings();
      services.AddSingleton(provider => Mapper.Instance);
      //services.AddTransient<ILocationHub, LocationHub>();
      services.AddTransient<ITeamService, TeamService>();
      services.AddTransient<IAdminService, AdminService>();
      services.AddTransient<IGameService, GameService>();
      services.AddTransient<IAuthService, AuthService>();
      services.AddTransient<IImageService, ImageService>();
      services.AddTransient<INodeService, NodeService>();
      services.AddTransient<IPlayerService, PlayerService>();
      services.AddTransient<IActionService, ActionService>();
      services.AddTransient<IPasscodeService, PasscodeService>();
      services.AddTransient<IImageTransformation, ImageTransformation>();
      services.AddTransient<IScoreChanger, ScoreDecreaseByTeamMember>();
      services.AddSignalR();
    }
    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
      //adding custom roles
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<Identity>>();
      string[] roleNames = Enum.GetNames(typeof(Role));
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        //creating the roles and seeding them to the database
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
          roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
      var context = serviceProvider.GetService<HuntContext>();
      var rootAdminEmail = Configuration["Admin:Email"];
      if (!context.Admins.Any(a => a.Email == rootAdminEmail))
      {
        var admin = new Admin() { Email = rootAdminEmail, Name = Configuration["Admin:Name"] };
        context.Admins.Add(admin);
        context.SaveChanges();
        //creating a super user who could maintain the web app
        var poweruser = new Identity()
        {
          UserName = admin.Name,
          Email = admin.Email,
          AppUserId = admin.Id
        };

        string UserPassword = Configuration["Admin:Password"];
        var _user = await UserManager.FindByEmailAsync(admin.Email);

        if (_user == null)
        {
          var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
          if (createPowerUser.Succeeded)
          {
            //here we tie the new user to the "Admin" role 
            await UserManager.AddToRoleAsync(poweruser, "Admin");
          }
        }
      }

      var botEmail = Configuration["BotConfiguration:BotEmail"];
      if (!context.Admins.Any(a => a.Email == botEmail))
      {
        var bot = new Admin() { Email = botEmail, Name = Configuration["BotConfiguration:BotName"] };
        context.Admins.Add(bot);
        context.SaveChanges();

        var botUser = new Identity()
        {
          UserName = bot.Name,
          Email = bot.Email,
          AppUserId = bot.Id
        };
        string botPassword = Configuration["BotConfiguration:BotPassword"];

        var _botUser = await UserManager.FindByEmailAsync(botUser.Email);
        if (_botUser == null)
        {
          var createBotUser = await UserManager.CreateAsync(botUser, botPassword);
          if (createBotUser.Succeeded)
          {
            await UserManager.AddToRoleAsync(botUser, Role.Bot.ToString());
          }
        }

      }
    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, HuntContext dbContext, IServiceProvider serviceProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      dbContext.Database.Migrate();
      //app.UseCors(builder=>builder.AllowAnyOrigin());
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
      app.UseAuthentication();
      app.UseMvc();
      app.UseSignalR(routes => { routes.MapHub<LocationHub>("/locationHub"); });
      CreateRoles(serviceProvider).Wait();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.RegisterType<UpdateNodePointsUpdater>().As<IUpdater>().Named<IUpdater>("UpdateNodePoints");
    }
    public static void ConfigureMappings()
    {
      Mapper.Reset();
      Mapper.Initialize(config =>
      {
        config.CreateMap<AddNodeRequest, Node>()
          .ConstructUsing(r => NodeFactory.CreateNode(r.NodeType));
        config.CreateMap<GameActionRequest, GameAction>()
          .ForMember(x => x.Picture, expression => expression.Ignore())
          .ForMember(m => m.PointsEarned, opt => opt.MapFrom(gar => gar.PointsEarned));
        config.CreateMap<GameAction, GameActionResponse>();
        config.CreateMap<Node, Node>().ForSourceMember(x => x.Id, opt => opt.DoNotValidate());
        config.CreateMap<Node, NodeResponse>()
          .Include<BonusNode, NodeResponse>()
          .Include<ChoiceNode, NodeResponse>()
          .Include<FirstNode, NodeResponse>()
          .Include<HiddenNode, NodeResponse>()
          .Include<LastNode, NodeResponse>()
          .Include<ObjectNode, NodeResponse>()
          .Include<PictureNode, NodeResponse>()
          .Include<QuestionNode, NodeResponse>()
          .Include<TimerNode, NodeResponse>()
          .Include<WaypointNode, NodeResponse>()
          .ForMember(n => n.ChildNodeIds, expression => expression.MapFrom(node => MapChildId(node)))
        ;
        config.CreateMap<BonusNode, NodeResponse>()
          .ForMember(n=>n.Hint, n=>n.MapFrom(node=>node.Location));
        config.CreateMap<ChoiceNode, NodeResponse>();
        config.CreateMap<FirstNode, NodeResponse>();
        config.CreateMap<HiddenNode, NodeResponse>()
          .ForMember(n => n.Hint, n => n.MapFrom(node => node.LocationHint));
          
        config.CreateMap<LastNode, NodeResponse>();
        config.CreateMap<ObjectNode, NodeResponse>();
        config.CreateMap<PictureNode, NodeResponse>();
        config.CreateMap<QuestionNode, NodeResponse>();
        config.CreateMap<TimerNode, NodeResponse>();
        config.CreateMap<WaypointNode, NodeResponse>();
        config.CreateMap<Answer, AnswerResponse>();
        config.CreateMap<GameAction, GameActionToValidate>()
          .ForMember(x => x.Node, x => x.Ignore());
        config.CreateMap<Team, TeamResponse>();
        config.CreateMap<Player, PlayerResponse>();
        config.CreateMap<Score, ScoreResponse>();
        config.CreateMap<Passcode, PasscodeResponse>();
        config.CreateMap<GameRequest, Game>();
        config.CreateMap<Admin, AdminResponse>()
          .ForMember(a => a.GameIds, a => a.MapFrom(admin => admin.Games.Select(g => g.Id)));
        config.CreateMap<PlayerRequest, Player>();
        config.CreateMap<Identity, UserResponse>();
        config.CreateMap<Game, GameTeamsResponse>()
          .ForMember(g => g.Teams, g => g.MapFrom(game => game.Teams))
          .ForMember(g => g.PictureId, g => g.MapFrom(game => game.Picture != null ? game.Picture.Id : 0));
        config.CreateMap<ImageRequest, Picture>()
          .ForMember(p=>p.Id, p=>p.MapFrom(i=>i.PictureId));
        config.CreateMap<Picture, ImageResponse>()
          .ForMember(p => p.PictureId, p => p.MapFrom(i => i.Id));
      });
    }

    private static IEnumerable<int> MapChildId(Node node)
    {
      var mapChildId = node.Children.Select(c => c.Id);
      return mapChildId;
    }
  }
}
