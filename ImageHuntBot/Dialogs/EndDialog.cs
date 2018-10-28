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

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.Status != Status.Started)
            {
                LogInfo< ImageHuntState>(turnContext, "The game had not started!");
                await turnContext.ReplyActivity("La partie n'a pas encore commencée, vous ne pouvez par l'arrêter!");
                await turnContext.End();
            }
            _logger.LogInformation($"The Hunt of GameId={state.GameId} for teamid={state.TeamId} had ended.");
            var gameActionRequest = new GameActionRequest()
            {
                Action = (int)ImageHuntWebServiceClient.Action.EndGame,
                GameId = state.GameId,
                TeamId = state.TeamId,
                Latitude = state.CurrentLatitude,
                Longitude = state.CurrentLongitude
            };
            state.Status = Status.Ended;
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.ReplyActivity(
                $"La chasse vient de prendre fin, vos actions ont été enregistrée et un orga va les valider.");
            await turnContext.End();
        }

        public override string Command => "/end";
    }
}