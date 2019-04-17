using System.Linq;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore.Commands;
using ImageHuntCore.Computation;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("resetNext")]
    public class ResetNextNodeCommand : AbstractCommand<ImageHuntState>, IResetNextNodeCommand
    {
        private readonly INodeWebService _nodeWebService;
        private readonly IGameWebService _gameWebService;

        public ResetNextNodeCommand(
            ILogger<IResetNextNodeCommand> logger, 
            IStringLocalizer<ResetNextNodeCommand> localizer, INodeWebService nodeWebService, 
            IGameWebService gameWebService) 
            : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
            _gameWebService = gameWebService;
        }

        public override bool IsAdmin => false;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                Logger.LogError("Game not started");
                await turnContext.SendActivityAsync(string.Format(Localizer["GAME_NOT_STARTED"]));
                return;
            }

            if (state.CurrentLocation == null)
            {
                Logger.LogError("No team localisation");
                await turnContext.SendActivityAsync(string.Format(Localizer["NO_LOCALIZATION"]));
                return;
            }

            var nodes = await _nodeWebService.GetNodesByType(NodeTypes.Path, state.GameId.Value);
            var closestNodes = nodes.OrderBy(n => GeographyComputation.Distance(
                state.CurrentLocation.Latitude.Value,
                state.CurrentLocation.Longitude.Value, n.Latitude, n.Longitude));
            state.CurrentNode = closestNodes.First();
            await turnContext.SendActivityAsync(Localizer["RESET_SUCCEED"]);
        }
    }
}