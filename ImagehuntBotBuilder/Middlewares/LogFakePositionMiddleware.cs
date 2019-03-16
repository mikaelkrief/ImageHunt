using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Middlewares
{
    public class LogFakePositionMiddleware : IMiddleware
    {
        private readonly ILogger<LogFakePositionMiddleware> _logger;
        private ImageHuntBotAccessors _accessors;

        public LogFakePositionMiddleware(ILogger<LogFakePositionMiddleware> logger, ImageHuntBotAccessors accessors)
        {
            _logger = logger;
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

        }
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var regex = new Regex(@"\/location lat=([0-9]*[.]?[0-9]+) lng=([0-9]*[.]?[0-9]+)");
            if (!turnContext.Activity.Text.IsNullOrEmpty() && regex.IsMatch(turnContext.Activity.Text))
            {
                var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());
                var group = regex.Matches(turnContext.Activity.Text);
                var latitude = Convert.ToDouble(group[0].Groups[1].Value, CultureInfo.InvariantCulture);
                var longitude = Convert.ToDouble(group[0].Groups[2].Value, CultureInfo.InvariantCulture);
                state.CurrentLocation = new GeoCoordinates(latitude: latitude, longitude:longitude);
                // Set the property using the accessor.
                await _accessors.ImageHuntState.SetAsync(turnContext, state);
                // Save the new turn count into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

            }
            else
            {
                await next.Invoke(cancellationToken);
            }
        }
    }
}