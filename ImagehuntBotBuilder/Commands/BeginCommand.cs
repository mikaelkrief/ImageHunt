using System.Linq;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
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
        private readonly INodeWebService _nodeWebService;

        public BeginCommand(IActionWebService actionWebService, 
            ITeamWebService teamWebService, 
            INodeWebService nodeWebService,
            ILogger<IBeginCommand> logger, 
            IStringLocalizer<BeginCommand> localizer) : base(logger, localizer)
        {
            _actionWebService = actionWebService;
            _teamWebService = teamWebService;
            _nodeWebService = nodeWebService;
        }

        public override bool IsAdmin => true;
        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            
            if (state.Status != Status.Initialized)
            {
                _logger.LogError("Game not initialized");
                await turnContext.SendActivityAsync(_localizer["CHAT_NOT_INITIALIZED"]);
                return;
            }

            if (state.CurrentLocation == null)
            {
                _logger.LogError("No location");
                await turnContext.SendActivityAsync(_localizer["NO_LIVE_LOCATION"]);
                return;
            }

            state.HiddenNodes = (await _nodeWebService.GetNodesByType(NodeTypes.Hidden, state.Game.Id)).ToArray();
            state.ActionNodes = (await _nodeWebService.GetNodesByType(NodeTypes.Action, state.Game.Id)).ToArray();
            var nextNode = await _teamWebService.StartGameForTeam(state.GameId.Value, state.TeamId.Value);
            state.CurrentNode = nextNode;
            state.CurrentNodeId = nextNode.Id;
            state.Status = Status.Started;
            var gameActionRequest = new GameActionRequest()
            {
                Action = (int)Action.StartGame,
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Latitude = state.CurrentLocation.Latitude,
                Longitude = state.CurrentLocation.Longitude
            };
            await _actionWebService.LogAction(gameActionRequest);
            await turnContext.SendActivityAsync(_localizer["GAME_STARTED"]);
        }
    }
}