using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class BeginDialog : AbstractDialog, IBeginDialog
    {
        private readonly IActionWebService _actionWebService;

        public BeginDialog(IActionWebService actionWebService, ILogger<BeginDialog> logger) : base(logger)
        {
            _actionWebService = actionWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.Status != Status.Initialized)
            {
                LogInfo<ImageHuntState>(turnContext, "Game not initialized");
                await turnContext.ReplyActivity("Le chat n'a pas été initialisé, impossible de commencer maintenant!");
                await turnContext.End();
                return;
            }
            LogInfo<ImageHuntState>(turnContext, "Start Game");

            var gameActionRequest = new GameActionRequest()
            {
                Action = (int) ImageHuntWebServiceClient.Action.StartGame,
                GameId = state.GameId,
                TeamId = state.TeamId,
                Latitude = state.CurrentLatitude,
                Longitude = state.CurrentLongitude
            };
            state.Status = Status.Started;
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.ReplyActivity($"La chasse commence maintenant! Bonne chance!");
            await turnContext.End();
        }

        public override string Command => "/begin";
    }
}