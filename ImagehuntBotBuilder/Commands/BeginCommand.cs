using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("begin")]
    public class BeginCommand : AbstractCommand, IBeginCommand
    {
        private readonly IActionWebService _actionWebService;
        private readonly ITeamWebService _teamWebService;

        public BeginCommand(IActionWebService actionWebService, ITeamWebService teamWebService,
            ILogger<IBeginCommand> logger, IStringLocalizer<BeginCommand> localizer)
            : base(logger, localizer)
        {
            _actionWebService = actionWebService;
            _teamWebService = teamWebService;
        }

        public override bool IsAdmin => true;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Initialized)
            {
                Logger.LogError("Game not initialized");
                await turnContext.SendActivityAsync(Localizer["CHAT_NOT_INITIALIZED"]);
                return;
            }

            if (state.CurrentLocation == null)
            {
                Logger.LogError("No location");
                await turnContext.SendActivityAsync(Localizer["NO_LIVE_LOCATION"]);
                return;
            }

            var nextNode = await _teamWebService.StartGameForTeam(state.GameId.Value, state.TeamId.Value);
            state.CurrentNode = nextNode;
            state.CurrentNodeId = nextNode.Id;
            state.Status = Status.Started;
            var gameActionRequest = new GameActionRequest
            {
                Action = (int)Action.StartGame,
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Latitude = state.CurrentLocation.Latitude,
                Longitude = state.CurrentLocation.Longitude
            };
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.SendActivityAsync(Localizer["GAME_STARTED"]);
        }
    }
}