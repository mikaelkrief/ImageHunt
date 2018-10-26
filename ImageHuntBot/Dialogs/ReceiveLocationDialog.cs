using System.Threading.Tasks;
using ImageHuntCore.Computation;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot.Dialogs
{
    public class ReceiveLocationDialog : AbstractDialog, IReceiveLocationDialog
    {
        private readonly IActionWebService _actionWebService;

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.Status == Status.None)
            {
                LogInfo<ImageHuntState>(turnContext, "Game not initialized");
                await turnContext.End();
                return;
            }
            state.CurrentLatitude = turnContext.Activity.Location.Latitude;
            state.CurrentLongitude = turnContext.Activity.Location.Longitude;

            _logger.LogInformation($"Received position: [lat:{state.CurrentLatitude}, lng:{state.CurrentLongitude}");
            var distance = GeographyComputation.Distance(state.CurrentLatitude, state.CurrentLongitude,
                state.CurrentNode.Latitude, state.CurrentNode.Longitude);
            if (distance <= 40.0)
            {
                await turnContext.ReplyActivity(
                    $"Bravo, vous avez rejoint le point de controle {state.CurrentNode.Name}");
            }
            await base.Begin(turnContext);
            var logPositionRequest = new LogPositionRequest()
            {
                GameId = state.GameId,
                TeamId = state.TeamId,
                Latitude = state.CurrentLatitude,
                Longitude = state.CurrentLongitude
            };
            await _actionWebService.LogPosition(logPositionRequest);
            //await turnContext.ReplyActivity(
            //  $"J'ai enregistré votre nouvelle position {state.CurrentLatitude}, {state.CurrentLongitude}");
            await turnContext.End();
        }

        public override string Command => "/location";

        public ReceiveLocationDialog(IActionWebService actionWebService, ILogger<ReceiveLocationDialog> logger) : base(logger)
        {
            _actionWebService = actionWebService;
        }
    }
}