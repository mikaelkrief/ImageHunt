using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                await turnContext.SendActivityAsync("Le groupe a déjà été initialisé!");
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
                _logger.LogInformation($"Init group for GameId={state.GameId} TeamId={state.TeamId}");
                state.Game = await _gameWebService.GetGameById(state.GameId.Value);
                state.Team = await _teamWebService.GetTeamById(state.TeamId.Value);
                if (state.Game == null || state.Team == null)
                {
                    _logger.LogError("Unable to find Game and/or Team");
                    await turnContext.SendActivityAsync(
                        $"Impossible de trouver la partie pour l'Id={state.GameId} ou l'équipe pour l'Id={state.TeamId}");
                    state.GameId = state.TeamId = null;
                    return;
                }

                var nodeResponses = await _nodeWebService.GetNodesByType(NodeTypes.Hidden, state.GameId.Value);
                state.HiddenNodes = new List<NodeResponse>(nodeResponses).ToArray();
                state.Status = Status.Initialized;
                await turnContext.SendActivityAsync(
                    $"Le groupe de l'équipe {state.Team.Name} pour la chasse {state.Game.Name} qui débute le {state.Game.StartDate.ToString(new CultureInfo(state.Team.CultureInfo))} est prêt, bon jeu!");
                _logger.LogInformation("Group initialized");
            }
        }
    }
}