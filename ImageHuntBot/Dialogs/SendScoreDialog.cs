using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class SendScoreDialog : AbstractDialog, ISendScoreDialog
    {
        private readonly IGameWebService _gameWebService;

        public SendScoreDialog(ILogger<SendScoreDialog> logger, IGameWebService gameWebService) : base(logger)
        {
            _gameWebService = gameWebService;
        }

        public override string Command => "/sendScore";
        public override async Task Begin(ITurnContext turnContext)
        {
            try
            {
                var regex = new Regex(@"\/sendScore\s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*))");
                var states = await turnContext.GetAllConversationState<ImageHuntState>();
                var gameIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;
                var teamIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
                IEnumerable<ImageHuntState> statesToBroadcast = null;
                var scoreBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(gameIdAsString))
                {
                    var gameId = Convert.ToInt32(gameIdAsString);
                    statesToBroadcast = states.Where(s => s.GameId == gameId);
                    var game = await _gameWebService.GetGameById(gameId);
                    var scores = await _gameWebService.GetScoresForGame(gameId);
                    scoreBuilder.Append($"Voici les scores pour la partie : {game.Name}").AppendLine();
                    scoreBuilder.Append($"Temps de parcours:").AppendLine();
                    scoreBuilder.Append("Equipe\tTemps de parcours\tScore");
                    foreach (var score in scores)
                    {
                        scoreBuilder.Append($"{score.Team.Name}\t{score.TravelTime}\t{score.Points}").AppendLine();
                    }
                }
                else if (!string.IsNullOrEmpty(teamIdAsString))
                {
                    var teamId = Convert.ToInt32(teamIdAsString);
                    statesToBroadcast = states.Where(s => s.TeamId == teamId);
                }
                foreach (var imageHuntState in statesToBroadcast)
                {
                    var activity = new Activity()
                    {
                        ChatId = imageHuntState.ChatId,
                        Text = scoreBuilder.ToString(),
                        ActivityType = ActivityType.Message
                    };
                    await turnContext.SendActivity(activity);
                }

            }
            finally
            {
                await turnContext.End();
            }
        }
    }
}