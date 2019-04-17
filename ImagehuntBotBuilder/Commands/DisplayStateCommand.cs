using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("state")]
    public class DisplayStateCommand : AbstractCommand<ImageHuntState>, IDisplayStateCommand
    {
        private readonly ImageHuntBotAccessors _accessors;

        public DisplayStateCommand(
            ILogger<IDisplayStateCommand> logger, 
            IStringLocalizer<DisplayStateCommand> localizer,
            ImageHuntBotAccessors accessors) : base(logger, localizer)
        {
            _accessors = accessors;
        }

        public override bool IsAdmin => true;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state != null)
            {
                var selectedStates = new List<ImageHuntState>();
                var regEx = new Regex(@"\/state\s?(gameid\s?=\s?(?'gameid'\d*)|(teamid\s?=\s?(?'teamid'\d*))|(?'all'all)|)");
                if (regEx.IsMatch(turnContext.Activity.Text))
                {
                    var matches = regEx.Match(turnContext.Activity.Text);
                     var states = await _accessors.AllStates.GetAllAsync();
                    // Strip null states
                   if (matches.Groups["all"].Success)
                   {
                       selectedStates.AddRange(states);
                    }
                    else if (matches.Groups["gameid"].Success)
                    {
                        var gameId = Convert.ToInt32(matches.Groups["gameid"].Captures[0].Value);
                        selectedStates.AddRange(states.Where(s => (s.Game != null && s.Game.Id == gameId) || 
                                                                  (s.GameId.HasValue && s.GameId.Value == gameId)));

                    }
                    else if (matches.Groups["teamid"].Success)
                    {
                        var teamid = Convert.ToInt32(matches.Groups["teamid"].Captures[0].Value);
                        selectedStates.AddRange(states.Where(s => (s.Team != null && s.Team.Id == teamid) || 
                                                                  (s.TeamId.HasValue && s.TeamId.Value == teamid)));

                    }
                    else
                    {
                        selectedStates.Add(state);
                    }

                }
                foreach (var imageHuntState in selectedStates)
                {
                    await ComposeReplyAsync(turnContext, imageHuntState);
                }

            }
        }

        private async Task ComposeReplyAsync(ITurnContext turnContext, ImageHuntState state)
        {
            var relyBuilder = new StringBuilder();
            relyBuilder.AppendLine($"ConversationId: {state.ConversationId}");
            if (state.Game != null)
            {
                relyBuilder.AppendLine(
                    $"Game: (Id:{state.Game.Id}, Name: {state.Game.Name}, StartDate: {state.Game.StartDate})");
            }
            relyBuilder.AppendLine($"Game Status: {state.Status}");

            if (state.Team != null)
            {
                relyBuilder.AppendLine(
                    $"Team: (Id: {state.Team.Id}, Name: {state.Team.Name}, Culture:{state.Team.CultureInfo})");
            }

            if (state.CurrentLocation != null)
            {
                relyBuilder.AppendLine(
                    $"CurrentLocation: (Lat:{state.CurrentLocation.Latitude}, Lng: {state.CurrentLocation.Longitude})");
            }

            if (state.CurrentNode != null)
            {
                string childs = string.Empty;
                if (state.CurrentNode.ChildNodeIds != null)
                {
                    childs = string.Join(',', state.CurrentNode.ChildNodeIds);
                }

                relyBuilder.AppendLine(
                    $"CurrentNode: (Id: {state.CurrentNode.Id}, Name: {state.CurrentNode.Name}, Location: [lat:{state.CurrentNode.Latitude}, {state.CurrentNode.Longitude}]) Childs: [{childs}]");
            }

            if (state.HiddenNodes != null && state.HiddenNodes.Any())
            {
                var hiddenNodes = string.Empty;
                hiddenNodes = string.Join(',', state.HiddenNodes.Select(n => n.Name));
                relyBuilder.AppendLine($"Hidden nodes: [{hiddenNodes}]");
            }

            await turnContext.SendActivityAsync(relyBuilder.ToString());
        }
    }
}