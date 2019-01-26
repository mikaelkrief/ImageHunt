using System.Threading.Tasks;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("list")]
    public class ListCommand : AbstractCommand, IListCommand
    {
        private readonly IActionWebService _actionWebService;

        public ListCommand(ILogger<IListCommand> logger, IStringLocalizer<ListCommand> localizer,
            IActionWebService actionWebService) 
            : base(logger, localizer)
        {
            _actionWebService = actionWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if ((!state.TeamId.HasValue || !state.GameId.HasValue))
            {
                _logger.LogError("Using ListCommand on non initialized group");
                return;
                
            }
            var actionRequest = new GameActionRequest()
            {
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Action = (int)Action.GivePoints,
                PointsEarned = -150,
                Validated = true,
            };
            await _actionWebService.LogAction(actionRequest);
        }

        public override bool IsAdmin => false;
    }
}