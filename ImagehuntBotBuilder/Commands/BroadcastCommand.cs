﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("broadcast")]
    public class BroadcastCommand : AbstractCommand, IBroadcastCommand
    {
        private readonly ImageHuntBotAccessors _accessors;

        public BroadcastCommand(ILogger<IBroadcastCommand> logger, ImageHuntBotAccessors accessors) : base(logger)
        {
            _accessors = accessors;
        }

        protected  override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            var regex = new Regex(@"\/broadcast\s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*)) (?'text'.*)");
            if (regex.IsMatch(turnContext.Activity.Text))
            {
                var states = await _accessors.AllStates.GetAllAsync();
                var gameIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;
                var teamIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
                var textToBroadcast = regex.Matches(turnContext.Activity.Text)[0].Groups["text"].Value;
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
                    var activity = new Activity()
                    {
                        Conversation = new ConversationAccount(id: imageHuntState.ConversationId),
                        Text = textToBroadcast,
                        Type = ActivityTypes.Message,
                        ServiceUrl = turnContext.Activity.ServiceUrl,
                        ChannelId = turnContext.Activity.ChannelId
                    };
                    await turnContext.SendActivityAsync(activity);
                }
            }
            else
            {
                await turnContext.SendActivityAsync("La syntaxe de la commande est éronée, veuillez vérifier");
                _logger.LogError($"Syntax error for command: {turnContext.Activity.Text}");
            }
        }
    }
}