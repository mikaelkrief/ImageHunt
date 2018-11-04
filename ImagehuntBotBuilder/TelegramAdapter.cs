using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.TransientFaultHandling;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHuntBotBuilder
{
    public class TelegramAdapter : BotFrameworkAdapter, IAdapterIntegration
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TelegramAdapter> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IConfiguration _configuration;

        public TelegramAdapter(IMapper mapper, 
            ILogger<TelegramAdapter> logger, 
            ITelegramBotClient telegramBotClient, 
            IConfiguration configuration, 
            ICredentialProvider credentialProvider)
            :base(credentialProvider)
        {
            _mapper = mapper;
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _configuration = configuration;
        }

        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            if (turnContext == null)
                throw new ArgumentNullException(nameof(turnContext));
            if (activities == null)
                throw new ArgumentNullException(nameof(activities));
            if (activities.Length == 0)
            {
                throw new ArgumentException("Expecting one or more activities, but the array was empty.", nameof(activities));
            }

            var responses = new ResourceResponse[activities.Length];
            for (int index = 0; index < activities.Length; index++)
            {
                var activity = activities[index];
                var response = default(ResourceResponse);
                switch (activity.ChannelId)
                {
                    case "emulator":
                        var connectorClient = turnContext.TurnState.Get<IConnectorClient>(typeof(IConnectorClient).FullName);
                        response = await connectorClient.Conversations
                            .SendToConversationAsync(activity, cancellationToken).ConfigureAwait(false);
                        if (response == null)
                        {
                            response = new ResourceResponse(activity.Id ?? string.Empty);
                        }

                        break;
                    case "telegram":
                        var telegramMessage = await _telegramBotClient.SendTextMessageAsync(Convert.ToInt64(activity.Conversation.Id), activity.Text);
                        response = new ResourceResponse(telegramMessage.MessageId.ToString());
                        break;
                }

                responses[index] = response;
            }

            return responses;
        }

        public virtual async Task<InvokeResponse> ProcessActivityAsync(string authHeader, Activity activity, BotCallbackHandler callback,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(activity.Type))
            {
                activity = _mapper.Map<Activity>(activity.Properties.ToObject<Update>());
                activity.ServiceUrl = _configuration["BotConfiguration:BotUrl"];
            }
            //else //emulator
            //{
            //    var regex = new Regex(@"\/location ");
            //    if (regex.IsMatch(activity.Text))
            //    {

            //    }
            //}
            return await base.ProcessActivityAsync(authHeader, activity, callback, cancellationToken);
            //BotAssert.ActivityNotNull(activity);
            //_logger.LogInformation($"Received an incoming activity.  ActivityId: {activity.Id}");

            //using (var context = new TurnContext(this, activity))
            //{

            //    if (activity.ChannelId != "telegram")
            //    {
            //        var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl), MicrosoftAppCredentials.Empty);
            //        context.TurnState.Add(connectorClient);

            //    }
            //    await RunPipelineAsync(context, callback, cancellationToken).ConfigureAwait(false);
            //}
            //return null;
        }
    }
}
