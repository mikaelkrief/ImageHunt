using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class StartDialog : AbstractDialog, IStartDialog
    {
        private readonly IActionWebService _actionWebService;

        public StartDialog(IActionWebService actionWebService, ILogger<StartDialog> logger) : base(logger)
        {
            _actionWebService = actionWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            _logger.LogInformation($"Start Hunt for GameId={state.GameId} and TeamId={state.TeamId}");
            var gameActionRequest = new GameActionRequest()
            {
                Action = (int) ImageHuntWebServiceClient.Action.StartGame,
                GameId = state.GameId,
                TeamId = state.TeamId,
                Latitude = state.CurrentLatitude,
                Longitude = state.CurrentLongitude
            };
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.ReplyActivity($"La chasse commence maintenant! Bonne chance!");
            await turnContext.End();
        }
    }
}