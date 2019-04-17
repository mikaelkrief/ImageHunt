using System;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore.Commands;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("hints")]
    public class DisplayHintsCommand : AbstractCommand<ImageHuntState>, IDisplayHintsCommand
    {
        private readonly INodeWebService _nodeWebService;
        private readonly ITeamWebService _teamWebService;
        public override bool IsAdmin => false;

        public DisplayHintsCommand(ILogger<IDisplayHintsCommand> logger, INodeWebService nodeWebService, IStringLocalizer<DisplayHintsCommand> localizer) : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    Localizer["GAME_NOT_STARTED"]);
                return;
            }

            var nodes = state.HiddenNodes;
            if (nodes == null || !nodes.Any())
            {
                await turnContext.SendActivityAsync(Localizer["NO_MORE_HIDDEN_NODE"]);
                return;
            }

            await turnContext.SendActivityAsync(Localizer["HIDDEN_NODES_TITLE"]);
            foreach (var nodeResponse in nodes)
            {
                switch (nodeResponse.NodeType)
                {
                    case NodeResponse.BonusNodeType:
                        string bonusType = string.Empty;
                        switch (nodeResponse.BonusType)
                        {
                            case BonusNode.BONUSTYPE.PointsX2:
                                bonusType = Localizer["2X_BONUS_TITLE"];
                                break;
                            case BonusNode.BONUSTYPE.PointsX3:
                                bonusType = Localizer["3X_BONUS_TITLE"];
                                break;
                        }
                        await turnContext.SendActivityAsync(Localizer["BONUS_HINT", nodeResponse.Hint, Environment.NewLine, bonusType]);
                        break;
                    case NodeResponse.HiddenNodeType:
                        await turnContext.SendActivityAsync(Localizer["HIDDEN_HINT", nodeResponse.Hint, Environment.NewLine, nodeResponse.Points]);
                        break;
                }
            }
        }
    }
}
