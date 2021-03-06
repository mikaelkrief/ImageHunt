﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
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

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (!state.GameId.HasValue || !state.TeamId.HasValue)
            {
                Logger.LogError("GameId or TeamId not set, unable to give points");
                await turnContext.SendActivityAsync(Localizer["CHAT_NOT_INITIALIZED"]);
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
                        PointsEarned = points,
                        Validated = true,
                    };
                    await _actionWebService.LogAction(request);
                    await turnContext.SendActivityAsync(string.Format(Localizer["GIVE_POINTS_MESSAGE"], points));
                    Logger.LogInformation("Admin give {0} to team {1}", points, state.TeamId);
                }
            }
            else
            {
                Logger.LogError("Game not started or ended, unable to give points");
                var warningMessage = Localizer["CHAT_NOT_INITIALIZED"];
                await turnContext.SendActivityAsync(warningMessage);
                return;
            }
        }
    }
}