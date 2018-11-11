﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImagehuntBotBuilder
{
    /// <summary>
    /// The Startup class configures services and the request pipeline.
    /// </summary>
    public class Startup
    {
        private ILoggerFactory _loggerFactory;
        private bool _isProduction = false;

        public Startup(IHostingEnvironment env)
        {
            _isProduction = env.IsProduction();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration that represents a set of key/value application configuration properties.
        /// </summary>
        /// <value>
        /// The <see cref="IConfiguration"/> that represents a set of key/value application configuration properties.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> specifies the contract for a collection of service descriptors.</param>
        /// <seealso cref="IStatePropertyAccessor{T}"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/azure/bot-service/bot-service-manage-channels?view=azure-bot-service-4.0"/>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMappings();

            var secretKey = Configuration.GetSection("botFileSecret")?.Value;
            var botFilePath = Configuration.GetSection("botFilePath")?.Value;

            // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
            var botConfig = BotConfiguration.Load(botFilePath ?? @".\BotConfiguration.bot", secretKey);
            services.AddSingleton(sp =>
                botConfig ??
                throw new InvalidOperationException($"The .bot config file could not be loaded. ({botConfig})"));

            // Retrieve current endpoint.
            var environment = _isProduction ? "production" : "development";
            var service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == environment).FirstOrDefault();
            IMultiStorage dataStore = new FileStorage(
                Configuration.GetValue<string>("BotConfiguration:StorageFolder"));
            services.AddSingleton(dataStore);
            if (!(service is EndpointService endpointService))
            {
                throw new InvalidOperationException(
                    $"The .bot file does not contain an endpoint with name '{environment}'.");
            }

            var credentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
            services.AddSingleton(credentialProvider);
            services.AddBot<ImageHuntBot>(options =>
            {
                options.CredentialProvider = credentialProvider;
                // Creates a logger for the application to use.
                ILogger logger = _loggerFactory.CreateLogger<ImageHuntBot>();

                // Catches any errors that occur during a conversation turn and logs them.
                options.OnTurnError = async (context, exception) =>
                {
                    logger.LogError($"Exception caught : {exception}");
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };


                // Create Conversation State object.
                // The Conversation State object is where we persist anything at the conversation-scope.
                var conversationState = new ConversationState(dataStore);

                options.State.Add(conversationState);
            });

            // Create and register state accesssors.
            // Acessors created here are passed into the IBot-derived class on every turn.
            services.AddSingleton<ImageHuntBotAccessors>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BotFrameworkOptions>>().Value;
                if (options == null)
                {
                    throw new InvalidOperationException(
                        "BotFrameworkOptions must be configured prior to setting up the state accessors");
                }

                var conversationState = options.State.OfType<ConversationState>().FirstOrDefault();
                if (conversationState == null)
                {
                    throw new InvalidOperationException(
                        "ConversationState must be defined and added before adding conversation-scoped state accessors.");
                }

                // Create the custom state accessor.
                // State accessors enable other components to read and write individual properties of state.
                var accessors = new ImageHuntBotAccessors(conversationState)
                {
                    ImageHuntState =
                        conversationState.CreateProperty<ImageHuntState>(ImageHuntBotAccessors.ImageHuntStateName),
                    AllStates = new MultiConversationState<ImageHuntState>(dataStore),
                };

                return accessors;
            });

            //services.AddTransient<IAdapterIntegration, TelegramAdapter>();
        }

        public void ConfigureProductionContainer(ContainerBuilder containerBuilder)
        {
            var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;

            containerBuilder.Register(context => new TelegramBotClient(botToken))
                .As<ITelegramBotClient>()
                .SingleInstance();
            containerBuilder.RegisterInstance(Configuration).AsImplementedInterfaces();

            containerBuilder.RegisterType<TelegramAdapter>().OnActivating(e =>
                {
                    var adapter = e.Instance;
                    adapter.Use(e.Context.Resolve<LogPositionMiddleware>());
                })
                .AsSelf()
                .As<IAdapterIntegration>();
            ConfigureContainer(containerBuilder);
        }

        public void ConfigureDevelopmentContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<LogFakePositionMiddleware>();
            containerBuilder.RegisterType<BotFrameworkAdapter>().OnActivating(e =>
            {
                var adapter = e.Instance;
                adapter.Use(e.Context.Resolve<LogFakePositionMiddleware>());
            }).As<IAdapterIntegration>();
            ConfigureContainer(containerBuilder);
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<DefaultModule>();
            var secretKey = Configuration.GetSection("botFileSecret")?.Value;
            var botFilePath = Configuration.GetSection("botFilePath")?.Value;

            // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
            var botConfig = BotConfiguration.Load(botFilePath ?? @".\BotConfiguration.bot", secretKey);
            containerBuilder.RegisterInstance(botConfig);

            // Retrieve current endpoint.
            var environment = _isProduction ? "production" : "development";
            var service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == environment).FirstOrDefault();
            if (!(service is EndpointService endpointService))
            {
                throw new InvalidOperationException(
                    $"The .bot file does not contain an endpoint with name '{environment}'.");
            }

            var credentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
            containerBuilder.RegisterInstance(credentialProvider).AsImplementedInterfaces();
            containerBuilder.RegisterInstance(Configuration);
            // Register bot client


            containerBuilder.RegisterType<LogPositionMiddleware>().AsSelf().As<IMiddleware>();
            containerBuilder.RegisterInstance(Mapper.Instance);
            containerBuilder.Register(a => new HttpClient()
            {
                BaseAddress = new Uri(Configuration.GetValue<string>("ImageHuntApi:Url")),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "ImageHuntBotToken") }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
            var telegramBotClient = app.ApplicationServices.GetService<ITelegramBotClient>();
            var botUrl = Configuration["BotConfiguration:BotUrl"];
            try
            {
                telegramBotClient?.SetWebhookAsync(botUrl).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #region Mapping Stuff
        private static string ActivityTypeFromUpdate(Update update)
        {
            var message = MessageFromUpdate(update);
            if (message.Location != null)
                return ImageHuntActivityTypes.Location;
            if (message.NewChatMembers != null && message.NewChatMembers.Length != 0)
                return ImageHuntActivityTypes.NewPlayer;
            return ActivityTypes.Message;
        }

        private static Message MessageFromUpdate(Update update)
        {
            if (update.Message != null)
                return update.Message;
            if (update.EditedMessage != null)
                return update.EditedMessage;
            return null;
        }

        public static void ConfigureMappings()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Update, Activity>()
                    .ForMember(a => a.ChannelId, opt => opt.UseValue("telegram"))
                    .ForMember(a => a.Value, opt => opt.MapFrom(u => u))
                    .ForMember(a => a.Timestamp,
                        opt => opt.ResolveUsing(u => new DateTimeOffset(MessageFromUpdate(u).Date)))
                    .ForMember(a => a.Id,
                        expression => expression.ResolveUsing(u => MessageFromUpdate(u).Chat.Id.ToString()))
                    .ForMember(a => a.Type, opt => opt.ResolveUsing(ActivityTypeFromUpdate))
                    .ForMember(a => a.Conversation, opt => opt.ResolveUsing(u =>
                    {
                        var message = MessageFromUpdate(u);
                        var conversation = new ConversationAccount() { Id = message.Chat.Id.ToString() };
                        return conversation;
                    }))
                    .ForMember(a => a.From, opt => opt.ResolveUsing(update =>
                    {
                        User from = MessageFromUpdate(update).From;
                        return new ChannelAccount(from.Id.ToString(), from.Username);
                    }))
                    .ForMember(a => a.Text, opt => opt.ResolveUsing(u => MessageFromUpdate(u).Text))
                    .ForMember(a => a.Attachments, opt => opt.ResolveUsing(u =>
                    {
                        var message = MessageFromUpdate(u);
                        var attachments = new List<Attachment>();
                        if (message.Photo != null)
                        {
                            var attachment = new Attachment()
                            {
                                ContentUrl = message.Photo.OrderByDescending(p => p.FileSize).First().FileId,
                                ContentType = "telegram/image",
                                Name = message.Text
                            };
                            attachments.Add(attachment);
                        }

                        if (message.Location != null)
                        {
                            var attachment = new Attachment()
                            {
                                ContentType = "location",
                                Content = new GeoCoordinates(latitude: message.Location.Latitude,
                                    longitude: message.Location.Longitude)
                            };
                            attachments.Add(attachment);
                        }

                        return attachments;
                    }))
                    //.ForMember(a=> a.From, expression => )
                    ;
            });
        }
        #endregion
    }
}