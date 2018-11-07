using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("begin")]

    public class BeginCommand : AbstractCommand, IBeginCommand
    {
        private readonly IActionWebService _actionWebService;
        private readonly ITeamWebService _teamWebService;

        public BeginCommand(IActionWebService actionWebService, ITeamWebService teamWebService, ILogger<IBeginCommand> logger) : base(logger)
        {
            _actionWebService = actionWebService;
            _teamWebService = teamWebService;
        }

        public override bool IsAdmin { get; }
        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Initialized)
            {
                _logger.LogError("Game not initialized");
                await turnContext.SendActivityAsync(
                    "Le chat n'a pas été initialisé, impossible de commencer maintenant!");
                return;
            }
            if (state.CurrentLocation == null)
            {
                _logger.LogError("No location");
                await turnContext.SendActivityAsync(
                    "Aucun joueur n'a activé sa localisation en continu, la chasse ne peut commencer!");
                return;
            }

            var nextNode = await _teamWebService.StartGameForTeam(state.GameId.Value, state.TeamId.Value);
            state.CurrentNode = nextNode;
            state.CurrentNodeId = nextNode.Id;
            state.Status = Status.Started;
            var gameActionRequest = new GameActionRequest()
            {
                Action = (int)ImageHuntWebServiceClient.Action.StartGame,
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Latitude = state.CurrentLocation.Latitude,
                Longitude = state.CurrentLocation.Longitude
            };
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.SendActivityAsync($"La chasse commence maintenant! Bonne chance!");
        }
    }
}