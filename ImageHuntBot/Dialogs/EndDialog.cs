using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class EndDialog : AbstractDialog, IEndDialog
    {
        private readonly IActionWebService _actionWebService;

        public EndDialog(IActionWebService actionWebService, ILogger<EndDialog> logger) : base(logger)
        {
            _actionWebService = actionWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            _logger.LogInformation($"The Hunt of GameId={state.GameId} for teamid={state.TeamId} had ended.");
            var gameActionRequest = new GameActionRequest()
            {
                Action = (int)ImageHuntWebServiceClient.Action.EndGame,
                GameId = state.GameId,
                TeamId = state.TeamId,
                Latitude = state.CurrentLatitude,
                Longitude = state.CurrentLongitude
            };

            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.ReplyActivity(
                $"La chasse vient de prendre fin, vos actions ont été enregistrée et un orga va les valider.");
            await turnContext.End();
        }
    }
}