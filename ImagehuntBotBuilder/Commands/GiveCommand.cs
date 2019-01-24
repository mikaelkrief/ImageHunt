using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHuntBotBuilder.Commands
{
    [Command("give")]
    public class GiveCommand : AbstractCommand, IGiveCommand
    {
        private readonly IActionWebService _actionWebService;

        public GiveCommand(ILogger<IGiveCommand> logger, IActionWebService actionWebService, IStringLocalizer<GiveCommand> localizer) : base(logger, localizer)
        {
            _actionWebService = actionWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (!state.GameId.HasValue || !state.TeamId.HasValue)
            {
                _logger.LogError("GameId or TeamId not set, unable to give points");
                await turnContext.SendActivityAsync(
                    "La chatroom n'a pas été correctement initialisée, impossible de donner des points!");
                return;
            }

            if (state.Status == Status.Started || state.Status == Status.Ended)
            {


                var regEx = new Regex(@"(?i)\/give points=(-?\d*)");
                var activityText = turnContext.Activity.Text;
                if (regEx.IsMatch(activityText))
                {
                    var groups = regEx.Matches(activityText);
                    var points = Convert.ToInt32(groups[0].Groups[1].Value);
                    var request = new GameActionRequest()
                    {
                        Action = (int)Action.GivePoints,
                        GameId = state.GameId.Value,
                        TeamId = state.TeamId.Value,
                        PointsEarned = points
                    };
                    await _actionWebService.LogAction(request);
                    await turnContext.SendActivityAsync($"L'orga vient de vous attribuer {points} points!");
                    _logger.LogInformation($"Admin give {points} to team {state.TeamId}");
                }
            }
            else
            {
                _logger.LogError("Game not started or ended, unable to give points");
                var warningMessage = $"La partie n'a pas le bon statut, impossible de donner des points à l'équipe";
                await turnContext.SendActivityAsync(warningMessage);
                return;
            }
        }
    }
}