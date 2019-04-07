using System.Linq;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("refresh")]
    public class RefreshCommand : AbstractCommand, IRefreshCommand
    {
        private readonly INodeWebService _nodeWebService;

        public RefreshCommand(ILogger<IRefreshCommand> logger, 
            IStringLocalizer<RefreshCommand> localizer, INodeWebService nodeWebService) 
            : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Game == null)
            {
                await turnContext.SendActivityAsync(_localizer["GROUP_NOT_INITIALIZED"]);
                return;
            }
            state.HiddenNodes = (await _nodeWebService.GetNodesByType(NodeTypes.Hidden, state.Game.Id)).ToArray();
            state.ActionNodes = (await _nodeWebService.GetNodesByType(NodeTypes.Action, state.Game.Id)).ToArray();
            await turnContext.SendActivityAsync(_localizer["REFRESH_HIDDEN_NODES"]);
            await turnContext.SendActivityAsync(_localizer["REFRESH_ACTION_NODES"]);
        }
    }
}