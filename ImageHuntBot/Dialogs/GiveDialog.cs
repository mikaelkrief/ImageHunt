using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using Action = ImageHuntWebServiceClient.Action;

namespace ImageHuntBot.Dialogs
{
    public class GiveDialog : AbstractDialog, IGiveDialog
    {
        private readonly IActionWebService _actionWebService;

        public GiveDialog(ILogger<GiveDialog> logger, IActionWebService actionWebService) : base(logger)
        {
            _actionWebService = actionWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.Status != Status.Started &&  state.Status != Status.Ended)
            {
                _logger.LogError("Game not started or ended, unable to give points");
                var warningMessage = $"La partie n'a pas le bon statut, impossible de donner des points à l'équipe";
                await turnContext.ReplyActivity(warningMessage);
                await turnContext.End();
                return;
            }
            var regEx = new Regex(@"(?i)\/give points=(\d*)");
            var activityText = turnContext.Activity.Text;
            if (regEx.IsMatch(activityText))
            {
                var groups = regEx.Matches(activityText);
                var points = Convert.ToInt32(groups[0].Groups[1].Value);
                var request = new GameActionRequest()
                {
                    Action = (int) Action.GivePoints, GameId = state.GameId, TeamId = state.TeamId, PointsEarned = points
                };
                await _actionWebService.LogAction(request);
                await turnContext.ReplyActivity($"L'orga vient de vous attribuer {points} points!");
            }

            await turnContext.End();
        }

        public override string Command => "/give";
    }
}