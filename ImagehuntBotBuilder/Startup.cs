﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ImageHuntBotBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigureMappings();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.RegisterInstance(Configuration);
            var botToken = Configuration.GetSection("BotConfiguration:BotToken").Value;
            // Register bot client
            containerBuilder.Register(context => new TelegramBotClient(botToken))
                .As<ITelegramBotClient>()
                .SingleInstance();
            services.AddBot<ImageHuntBot>(options =>
            {
                var secretKey = Configuration.GetSection("botFileSecret")?.Value;
                var botFilePath = Configuration.GetSection("botFilePath")?.Value;

                // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
                var botConfig = BotConfiguration.Load(botFilePath ?? @".\BotConfiguration.bot", secretKey);
                services.AddSingleton(sp => botConfig ?? throw new InvalidOperationException($"The .bot config file could not be loaded. ({botConfig})"));

                // Retrieve current endpoint.
                var environment = _isProduction ? "production" : "development";
                var service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == environment).FirstOrDefault();
                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException($"The .bot file does not contain an endpoint with name '{environment}'.");
                }

                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

                // Creates a logger for the application to use.
                ILogger logger = _loggerFactory.CreateLogger<ImageHuntBot>();

                // Catches any errors that occur during a conversation turn and logs them.
                options.OnTurnError = async (context, exception) =>
                {
                    logger.LogError($"Exception caught : {exception}");
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };

                // The Memory Storage used here is for local bot debugging only. When the bot
                // is restarted, everything stored in memory will be gone.
                IStorage dataStore = new MemoryStorage();

                // For production bots use the Azure Blob or
                // Azure CosmosDB storage providers. For the Azure
                // based storage providers, add the Microsoft.Bot.Builder.Azure
                // Nuget package to your solution. That package is found at:
                // https://www.nuget.org/packages/Microsoft.Bot.Builder.Azure/
                // Uncomment the following lines to use Azure Blob Storage
                // //Storage configuration name or ID from the .bot file.
                // const string StorageConfigurationId = "<STORAGE-NAME-OR-ID-FROM-BOT-FILE>";
                // var blobConfig = botConfig.FindServiceByNameOrId(StorageConfigurationId);
                // if (!(blobConfig is BlobStorageService blobStorageConfig))
                // {
                //    throw new InvalidOperationException($"The .bot file does not contain an blob storage with name '{StorageConfigurationId}'.");
                // }
                // // Default container name.
                // const string DefaultBotContainer = "<DEFAULT-CONTAINER>";
                // var storageContainer = string.IsNullOrWhiteSpace(blobStorageConfig.Container) ? DefaultBotContainer : blobStorageConfig.Container;
                // IStorage dataStore = new Microsoft.Bot.Builder.Azure.AzureBlobStorage(blobStorageConfig.ConnectionString, storageContainer);

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
                    throw new InvalidOperationException("BotFrameworkOptions must be configured prior to setting up the state accessors");
                }

                var conversationState = options.State.OfType<ConversationState>().FirstOrDefault();
                if (conversationState == null)
                {
                    throw new InvalidOperationException("ConversationState must be defined and added before adding conversation-scoped state accessors.");
                }

                // Create the custom state accessor.
                // State accessors enable other components to read and write individual properties of state.
                var accessors = new ImageHuntBotAccessors(conversationState)
                {
                    ImageHuntState = conversationState.CreateProperty<ImageHuntState>(ImageHuntBotAccessors.ImageHuntStateName),
                };

                return accessors;
            });
            services.AddTransient<IAdapterIntegration, TelegramAdapter>();

            containerBuilder.RegisterType<TelegramAdapter>().As<IAdapterIntegration>();
            containerBuilder.RegisterInstance(Mapper.Instance);
            containerBuilder.Populate(services);
            //services.AddSingleton(Mapper.Instance);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }

        private static string ActivityTypeFromUpdate(Update update)
        {
            if ((update.Message != null && update.Message.Location != null) ||
                (update.EditedMessage != null && update.EditedMessage.Location != null))
                return "location";
            return "message";
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
                    .ForMember(a => a.Timestamp, opt => opt.ResolveUsing(u => new DateTimeOffset(MessageFromUpdate(u).Date)))
                    .ForMember(a => a.Id, expression => expression.ResolveUsing(u => MessageFromUpdate(u).Chat.Id.ToString()))
                    .ForMember(a => a.Type, opt => opt.ResolveUsing(ActivityTypeFromUpdate))
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
                                  ContentType = "image",
                                  Name = message.Text
                              };
                              attachments.Add(attachment);
                          }

                          return attachments;
                      }))
                    //.ForMember(a=> a.From, expression => )
                    ;
            });
        }
    }
}
