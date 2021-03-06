﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Middlewares
{
    public class LogPositionMiddleware : IMiddleware
    {
        private readonly ILogger<LogPositionMiddleware> _logger;
        private readonly ImageHuntBotAccessors _accessors;
        private readonly IActionWebService _actionWebService;

        public LogPositionMiddleware(
            ILogger<LogPositionMiddleware> logger,
            ImageHuntBotAccessors accessors,
            IActionWebService actionWebService)
        {
            _logger = logger;
            _accessors = accessors;
            _actionWebService = actionWebService;
        }

        public async Task OnTurnAsync(
            ITurnContext turnContext, 
            NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (turnContext.Activity.Type == ImageHuntActivityTypes.Location)
            {
                var state = await _accessors.ImageHuntState.GetAsync(turnContext, cancellationToken: cancellationToken);
                if (state.GameId.HasValue &&
                    state.TeamId.HasValue)
                {
                    var location = turnContext.Activity.Attachments.Single().Content as GeoCoordinates;
                    _logger.LogInformation(
                        "Receive location [{0}, {1}] for GameId={2}, TeamId={3}", location.Latitude, location.Longitude, state.GameId, state.TeamId);
                    var logPositionRequest = new LogPositionRequest()
                    {
                        GameId = state.GameId.Value,
                        TeamId = state.TeamId.Value,
                        Latitude = location.Latitude ?? 0d,
                        Longitude = location.Longitude ?? 0d,
                    };
                    await _actionWebService.LogPosition(logPositionRequest, cancellationToken);
                    state.CurrentLocation = location;
                    // Set the property using the accessor.
                    await _accessors.ImageHuntState.SetAsync(turnContext, state);
                    // Save the new turn count into the conversation state.
                    await _accessors.ConversationState.SaveChangesAsync(turnContext);
                }
            }
            await next(cancellationToken);
        }
    }
}