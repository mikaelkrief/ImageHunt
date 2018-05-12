using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot.Dialogs.Prompts;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImageHuntTelegramBot.Dialogs
{
  public class InitDialog : AbstractDialog, IInitDialog
  {
    private readonly IGameWebService _gameWebService;
    private readonly ITeamWebService _teamWebService;

    public InitDialog(IGameWebService gameWebService, ITeamWebService teamWebService)
    {
      _gameWebService = gameWebService;
      _teamWebService = teamWebService;
    }

    public override async Task Begin(ITurnContext turnContext)
    {
      var state = turnContext.GetConversationState<ImageHuntState>();
      if (state.GameId != 0 && state.TeamId != 0)
      {
        await turnContext.ReplyActivity("Le groupe à déjà été initialisé!");
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
        state.Game = await _gameWebService.GetGameById(state.GameId);
        state.Team = await _teamWebService.GetTeamById(state.TeamId);
        
        
      }
      await base.Begin(turnContext);
      await turnContext.ReplyActivity(
        $"Le groupe de l'équipe {state.Team.Name} pour la chasse {state.Game.Name} qui débute le {state.Game.StartDate} est prêt, bon jeu!");
      await turnContext.End();
    }
  }
}