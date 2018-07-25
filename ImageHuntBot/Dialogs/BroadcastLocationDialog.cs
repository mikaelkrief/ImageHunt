using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace ImageHuntBot.Dialogs
{
    public class BroadcastLocationDialog : AbstractDialog, IBroadcastLocationDialog
    {
        public BroadcastLocationDialog(ILogger<BroadcastLocationDialog> logger) 
            : base(logger)
        {
        }
        public override async Task Begin(ITurnContext turnContext)
        {
            //_logger.LogInformation();
            try
            {
                var regEx = new Regex(@"\/broadcastLocation\s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*))\s*Lat=(?'Lat'[\d,\.]*)\s*Lng=(?'Lng'[\d,\.]*)");
                var states = await turnContext.GetAllConversationState<ImageHuntState>();
                var gameIdAsString = regEx.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;
                var teamIdAsString = regEx.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
                var latitudeToBroadcast = regEx.Matches(turnContext.Activity.Text)[0].Groups["Lat"].Value;
                var latitude = Convert.ToDouble(latitudeToBroadcast, CultureInfo.InvariantCulture);
                var longitudeToBroadcast = regEx.Matches(turnContext.Activity.Text)[0].Groups["Lng"].Value;
                var longitude = Convert.ToDouble(longitudeToBroadcast, CultureInfo.InvariantCulture);
                IEnumerable<ImageHuntState> statesToBroadcast = null;
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
                    var activity = new Activity() { ChatId = imageHuntState.ChatId, Location = new Location(){Latitude = (float)latitude, Longitude = (float)longitude},
                        ActivityType = ActivityType.Message };
                    await turnContext.SendActivity(activity);
                }

            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/BroadcastLocation";
    }
}
