using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("init")]
    public class InitCommand : AbstractCommand, IInitCommand
    {
        private readonly IGameWebService _gameWebService;
        private readonly ITeamWebService _teamWebService;
        private readonly INodeWebService _nodeWebService;

        public InitCommand(ILogger<IInitCommand> logger, IGameWebService gameWebService,
            ITeamWebService teamWebService, INodeWebService nodeWebService, IStringLocalizer<InitCommand> localizer) : base(logger, localizer)
        {
            _gameWebService = gameWebService;
            _teamWebService = teamWebService;
            _nodeWebService = nodeWebService;
        }

        public override bool IsAdmin => true;

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.None || state.GameId.HasValue || state.TeamId.HasValue)
            {
                await turnContext.SendActivityAsync(_localizer["GROUP_ALREADY_INITIALIZED"]);
                _logger.LogWarning("Group already initialized");
                return;
            }

            var text = turnContext.Activity.Text;
            var regEx = new Regex(@"(?i)\/init gameid=(\d*) teamid=(\d*)");
            if (regEx.IsMatch(text))
            {
                var groups = regEx.Matches(text);
                state.GameId = Convert.ToInt32(groups[0].Groups[1].Value);
                state.TeamId = Convert.ToInt32(groups[0].Groups[2].Value);
                _logger.LogInformation("Init group for GameId={0} TeamId={1}", state.GameId, state.TeamId);
                state.Game = await _gameWebService.GetGameById(state.GameId.Value);
                state.Team = await _teamWebService.GetTeamById(state.TeamId.Value);
                if (state.Game == null || state.Team == null)
                {
                    _logger.LogError("Unable to find Game and/or Team");

                    var unableToFindGame = string.Format(_localizer["UNABLE_FIND_GAME"], state.GameId??0, state.TeamId??0);
                    await turnContext.SendActivityAsync(unableToFindGame);
                    state.GameId = state.TeamId = null;
                    return;
                }

                var nodeResponses = await _nodeWebService.GetNodesByType(NodeTypes.Hidden, state.GameId.Value);
                state.HiddenNodes = new List<NodeResponse>(nodeResponses).ToArray();
                state.Status = Status.Initialized;
                string confirmMessage =
                    string.Format(_localizer["GROUP_INITIALIZED"],
                        state.Team.Name, state.Game.Name, state.Game.StartDate.ToString(new CultureInfo(state.Team.CultureInfo)));
                await turnContext.SendActivityAsync(confirmMessage);
                _logger.LogInformation("Group initialized");
            }
        }
    }
}