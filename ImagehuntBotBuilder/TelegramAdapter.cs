using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.TransientFaultHandling;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHuntBotBuilder
{
    public class TelegramAdapter : BotAdapter, IAdapterIntegration
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TelegramAdapter> _logger;
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramAdapter(IMapper mapper, ILogger<TelegramAdapter> logger, ITelegramBotClient telegramBotClient)
        {
            _mapper = mapper;
            _logger = logger;
            _telegramBotClient = telegramBotClient;
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
                        var connectorClient = turnContext.TurnState.Get<IConnectorClient>(typeof(ConnectorClient).FullName);
                        response = await connectorClient.Conversations
                            .SendToConversationAsync(activity, cancellationToken).ConfigureAwait(false);
                        if (response == null)
                        {
                            response = new ResourceResponse(activity.Id ?? string.Empty);
                        }

                        break;
                    case "telegram":
                        await _telegramBotClient.SendTextMessageAsync(Convert.ToInt64(activity.Id), activity.Text);
                        break;
                }

                responses[index] = response;
            }
            return responses;
        }

        public override async Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<InvokeResponse> ProcessActivityAsync(string authHeader, Activity activity, BotCallbackHandler callback,
            CancellationToken cancellationToken)
        {
            BotAssert.ActivityNotNull(activity);
            _logger.LogInformation($"Received an incoming activity.  ActivityId: {activity.Id}");
            if (string.IsNullOrEmpty(activity.Type))
            {
                // Extract activity from properties using Telegram.Bot library
                var update = activity.Properties.ToObject<Update>();
                activity = _mapper.Map<Activity>(update);
            }

            using (var context = new TurnContext(this, activity))
            {

                if (activity.ChannelId != "telegram")
                {
                    var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl), MicrosoftAppCredentials.Empty);
                    context.TurnState.Add(connectorClient);

                }
                await RunPipelineAsync(context, callback, cancellationToken).ConfigureAwait(false);
            }
            return null;
        }
    }
}
