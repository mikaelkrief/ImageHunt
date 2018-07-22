using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImageHuntTelegramBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace ImageHuntBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Add Autofac as dependency injection
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.RegisterInstance(Configuration);
            var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
            containerBuilder.RegisterInstance(new HttpClient()
            {
                BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url")),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "ImageHuntBotToken") }
            });
            containerBuilder.RegisterInstance(
                new FileStorage(Configuration.GetValue<string>("BotConfiguration:StorageFolder"))).As<IStorage>();
            containerBuilder.RegisterInstance(new TelegramBotClient(botToken)).As<ITelegramBotClient>();

            containerBuilder.Populate(services);

            var container = containerBuilder.Build();

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
