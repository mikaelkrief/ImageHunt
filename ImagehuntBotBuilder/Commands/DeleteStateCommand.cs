using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("delState")]
    public class DeleteStateCommand : AbstractCommand, IDeleteStateCommand
    {
        private readonly ImageHuntBotAccessors _accessors;

        public DeleteStateCommand(
            ILogger<IDeleteStateCommand> logger, 
            IStringLocalizer<DeleteStateCommand> localizer,
            ImageHuntBotAccessors accessors) : base(logger, localizer)
        {
            _accessors = accessors;
        }

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            var regex = new Regex(@"\/delState \s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*))");
            if (regex.IsMatch(turnContext.Activity.Text))
            {
                var gameIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;

                var teamIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
                var states = await _accessors.AllStates.GetAllAsync();

                if (!string.IsNullOrEmpty(gameIdAsString))
                {
                    var gameId = Convert.ToInt32(gameIdAsString);
                    var statesToDelete = states.Where(s => s.GameId == gameId);
                    foreach (var imageHuntState in statesToDelete)
                    {
                        var deleteContext = new TurnContext(turnContext.Adapter,new Activity());
                        deleteContext.TurnState.Add(imageHuntState);
                        await _accessors.DeleteStateAsync(deleteContext);
                    }
                }
                if (!string.IsNullOrEmpty(teamIdAsString))
                {
                    var teamId = Convert.ToInt32(teamIdAsString);
                    var statesToDelete = states.Where(s => s.TeamId == teamId);
                    foreach (var imageHuntState in statesToDelete)
                    {
                        var deleteContext = new TurnContext(turnContext.Adapter,new Activity());
                        deleteContext.TurnState.Add(imageHuntState);
                        await _accessors.DeleteStateAsync(deleteContext);
                    }
                }
            }
            
        }
    }
}