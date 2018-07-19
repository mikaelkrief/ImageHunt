using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot.Dialogs.Prompts;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ImageHuntTelegramBot.Dialogs
{
    public class InitDialog : AbstractDialog, IInitDialog
    {
        private readonly IGameWebService _gameWebService;
        private readonly ITeamWebService _teamWebService;

        public InitDialog(IGameWebService gameWebService, ITeamWebService teamWebService, ILogger<InitDialog> logger) : base(logger)
        {
            _gameWebService = gameWebService;
            _teamWebService = teamWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.GameId != 0 && state.TeamId != 0)
            {
                var warningMessage = $"Le groupe {turnContext.ChatId} a déjà été initialisé!";
                await turnContext.ReplyActivity(warningMessage);
                _logger.LogWarning(warningMessage);
                await turnContext.End();
                return;
            }
            var regEx = new Regex(@"(?i)\/init gameid=(\d*) teamid=(\d*)");
            var activityText = turnContext.Activity.Text;
            if (regEx.IsMatch(activityText))
            {
                var groups = regEx.Matches(activityText);
                state.GameId = Convert.ToInt32(groups[0].Groups[1].Value);
                state.TeamId = Convert.ToInt32(groups[0].Groups[2].Value);
                _logger.LogInformation($"Init game for gameId: {state.GameId}, teamId: {state.TeamId}");
                state.Game = await _gameWebService.GetGameById(state.GameId);
                state.Team = await _teamWebService.GetTeamById(state.TeamId);
                if (state.Game == null || state.Team == null)
                {
                    _logger.LogError($"Unable to find game for {state.GameId} or team for {state.TeamId}");
                    await turnContext.ReplyActivity($"Impossible de trouver la partie pour l'Id={state.GameId} ou l'équipe pour l'Id={state.TeamId}");
                    state.GameId = state.TeamId = 0;
                    await turnContext.End();
                    return;
                }

            }
            await base.Begin(turnContext);
            await turnContext.ReplyActivity(
              $"Le groupe de l'équipe {state.Team.Name} pour la chasse {state.Game.Name} qui débute le {state.Game.StartDate} est prêt, bon jeu!");
            await turnContext.End();
        }
    }
}