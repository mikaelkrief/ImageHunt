using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class BroadcastDialog : AbstractDialog, IBroadcastDialog
    {
        public BroadcastDialog(ILogger<BroadcastDialog> logger) : base(logger)
        {
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            //_logger.LogInformation();
            var regEx = new Regex(@"\/broadcast\s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*)) (?'text'.*)");
            var states = await turnContext.GetAllConversationState<ImageHuntState>();
            var gameIdAsString = regEx.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;
            var teamIdAsString = regEx.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
            var textToBroadcast = regEx.Matches(turnContext.Activity.Text)[0].Groups["text"].Value;
            IEnumerable<ImageHuntState> statesToBroadcast =null;
            if (!string.IsNullOrEmpty(gameIdAsString))
            {
                var gameId = Convert.ToInt32(gameIdAsString);
                statesToBroadcast = states.Where(s => s.GameId == gameId);
            }
            else if (!string.IsNullOrEmpty(teamIdAsString))
            {
                var teamId = Convert.ToInt32(teamIdAsString);
                statesToBroadcast = states.Where(s => s.TeamId == teamId);
            }

            foreach (var imageHuntState in statesToBroadcast)
            {
                var activity = new Activity(){ChatId = imageHuntState.ChatId, Text = textToBroadcast };
                await turnContext.SendActivity(activity);
            }

            await turnContext.End();
        }
    }
}