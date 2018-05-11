using System;
using System.Threading;
using System.Threading.Tasks;
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
      var firstStep = new NumberPrompt<int>("Merci de m'indiquer l'id de la partie", PromptGameId);
      AddChildren(firstStep);
      var secondStep = new NumberPrompt<int>("Merci de m'indiquer l'id de l'équipe", PromptTeamNumber);
      AddChildren(secondStep);
    }

    private async Task PromptTeamNumber(ITurnContext context, object result)
    {
      var state = context.GetConversationState<ImageHuntState>();
      state.TeamId = (int) result;
    }

    private async Task PromptGameId(ITurnContext context, object result)
    {
      var state = context.GetConversationState<ImageHuntState>();
      state.GameId = (int) result;
      using (var cancellationTokenSource = new CancellationTokenSource(100000))
      {
        state.Game = await _gameWebService.GetGameById(state.GameId, cancellationTokenSource.Token);
      }
    }
  }
}