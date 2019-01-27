using System.Linq;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("hints")]
    public class DisplayHintsCommand : AbstractCommand, IDisplayHintsCommand
    {
        private readonly INodeWebService _nodeWebService;
        private readonly ITeamWebService _teamWebService;
        public override bool IsAdmin => false;

        public DisplayHintsCommand(ILogger<IDisplayHintsCommand> logger, INodeWebService nodeWebService, IStringLocalizer<DisplayHintsCommand> localizer) : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    _localizer["GAME_NOT_STARTED"]);
                return;
            }

            var nodes = state.HiddenNodes;
            if (nodes == null || !nodes.Any())
            {
                await turnContext.SendActivityAsync(_localizer["NO_MORE_HIDDEN_NODE"]);
                return;
            }

            await turnContext.SendActivityAsync(_localizer["HIDDEN_NODES_TITLE"]);
            foreach (var nodeResponse in nodes)
            {
                switch (nodeResponse.NodeType)
                {
                    case NodeResponse.BonusNodeType:
                        string bonusType = string.Empty;
                        switch (nodeResponse.BonusType)
                        {
                            case BonusNode.BONUS_TYPE.Points_x2:
                                bonusType = _localizer["2X_BONUS_TITLE"];
                                break;
                            case BonusNode.BONUS_TYPE.Points_x3:
                                bonusType = _localizer["3X_BONUS_TITLE"];
                                break;
                        }
                        await turnContext.SendActivityAsync(string.Format(_localizer["BONUS_HINT"], nodeResponse.Hint, bonusType));
                        break;
                    case NodeResponse.HiddenNodeType:
                        await turnContext.SendActivityAsync(string.Format(_localizer["HIDDEN_HINT"], nodeResponse.Hint, nodeResponse.Points));
                        break;
                }
            }
        }
    }
}
