using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Computation;
using ImageHunt.Controllers;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

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
        //options.AddPolicy("ApiUser", new AuthorizationPolicyBuilder()
        //  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        //  .RequireAuthenticatedUser().Build());
      });
      //services.AddCors();
      services.AddMvc()
        .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
      //services.AddTransient<IAuthenticationHandler, TokenAuthenticationHandler>();
      //services.AddTransient<IAuthorizationHandler, TokenAuthorizationHandler>();
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
      //var context = serviceProvider.GetService<HuntContext>();
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
      if (!context.Admins.Any(a=>a.Email == rootAdminEmail))
      {
        var admin = new Admin() {Email = rootAdminEmail, Name = Configuration["Admin:Name"] };
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

    public static void ConfigureMappings()
    {
      Mapper.Reset();
      Mapper.Initialize(config =>
      {
        config.CreateMap<AddNodeRequest, Node>()
          .ConstructUsing(r => NodeFactory.CreateNode(r.NodeType));
        config.CreateMap<GameActionRequest, GameAction>()
          .ForMember(x => x.Picture, expression => expression.Ignore())
          .ForMember(m=>m.PointsEarned, opt=>opt.MapFrom(gar=>gar.PointsEarned));
        config.CreateMap<GameAction, GameActionResponse>();
          //.ForMember(m=>m.GameId, opt=>opt.MapFrom(ga=>ga.Game.Id));
        config.CreateMap<Node, Node>().ForSourceMember(x => x.Id, opt => opt.DoNotValidate());
        config.CreateMap<Node, NodeResponse>()
          .ForMember(n=>n.ChildNodeIds, expression => expression.MapFrom(node => node.Children.Select(c=>c.Id)))
          .ForMember(n=>n.Hint, expr=>
            expr.MapFrom(node => node.NodeType == NodeResponse.HiddenNodeType? (node as HiddenNode).LocationHint: (node as BonusNode).Location)
          );
        config.CreateMap<ObjectNode, NodeResponse>()
          .ForMember(n => n.ChildNodeIds, expression => expression.MapFrom(node => node.Children.Select(c => c.Id)));
        config.CreateMap<ChoiceNode, NodeResponse>()
          .ForMember(n => n.ChildNodeIds, expression => expression.MapFrom(node => node.Children.Select(c => c.Id)))
          .ForMember(n=>n.Question, exp=>exp.MapFrom(node =>node.Choice))
          .ForMember(n => n.Answers, exp=>exp.MapFrom(node=>node.Answers));
        config.CreateMap<Answer, AnswerResponse>();
        config.CreateMap<GameAction, GameActionToValidate>()
          .ForMember(x=>x.Node, x=>x.Ignore());
        config.CreateMap<Team, TeamResponse>();
        config.CreateMap<Player, PlayerResponse>();
        config.CreateMap<Score, ScoreResponse>();
        config.CreateMap<Passcode, PasscodeResponse>();
        config.CreateMap<GameRequest, Game>();
        config.CreateMap<Admin, AdminResponse>()
          .ForMember(a=>a.GameIds, a=>a.MapFrom(admin => admin.Games.Select(g=>g.Id)));
        config.CreateMap<PlayerRequest, Player>();
        config.CreateMap<PictureNode, NodeResponse>()
          .ForPath(n=>n.Image.Id, o=>o.MapFrom(p=>p.Image.Id))  
          .ForPath(n => n.Image, o => o.Ignore());
        config.CreateMap<Identity, UserResponse>();
        config.CreateMap<Game, GameTeamsResponse>()
          .ForMember(g=>g.Teams, g=>g.MapFrom(game=>game.Teams))
          .ForMember(g=>g.PictureId, g=>g.MapFrom(game=>game.Picture != null? game.Picture.Id:0));
      });
    }
  }
}
