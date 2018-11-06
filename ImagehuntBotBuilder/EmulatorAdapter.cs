using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;

namespace ImageHuntBotBuilder
{
    public class EmulatorAdapter : BotAdapter, IAdapterIntegration
    {
        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities,
            CancellationToken cancellationToken)
        {
            var responses = new List<ResourceResponse>();
            foreach (var activity in activities)
            {
                var response = default(ResourceResponse);

                var connectorClient = turnContext.TurnState.Get<IConnectorClient>();
                response = await connectorClient.Conversations.ReplyToActivityAsync(activity, cancellationToken).ConfigureAwait(false);
                responses.Add(response);
            }

            return responses.ToArray();
        }

        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<InvokeResponse> ProcessActivityAsync(string authHeader, Activity activity,
            BotCallbackHandler callback,
            CancellationToken cancellationToken)
        {
            var regex = new Regex(@"\/location lat=([0-9]*[.]?[0-9]+) lng=([0-9]*[.]?[0-9]+)");

            if (regex.IsMatch(activity.Text))
            {
                activity.Type = ImageHuntActivityTypes.Location;
                var groups = regex.Matches(activity.Text);
                var lat = Convert.ToDouble(groups[0].Groups[1].Value, CultureInfo.InvariantCulture);
                var lng = Convert.ToDouble(groups[0].Groups[2].Value, CultureInfo.InvariantCulture);
                activity.Attachments = new List<Attachment>(){new Attachment(contentType: ImageHuntActivityTypes.Location, content:new GeoCoordinates(latitude:lat, longitude:lng)) };
            }
            using (var turnContext = new TurnContext(this, activity))
            {
                await base.RunPipelineAsync(turnContext, callback, cancellationToken);
                return null;
            }
        }
    }
}