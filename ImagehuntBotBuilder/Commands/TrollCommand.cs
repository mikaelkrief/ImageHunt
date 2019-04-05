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
    [Command("troll")]
    public class TrollCommand : AbstractCommand, ITrollCommand
    {
        private readonly IActionWebService _actionWebService;

        public TrollCommand(ILogger<ITrollCommand> logger, IStringLocalizer<TrollCommand> localizer,
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
            await turnContext.SendActivityAsync(
                $"Félicitation! Vous avez utilisé la commande Troll! Pour votre peine, je vous fais bénéficier de {actionRequest.PointsEarned} points!");
        }

        public override bool IsAdmin => false;
    }
}