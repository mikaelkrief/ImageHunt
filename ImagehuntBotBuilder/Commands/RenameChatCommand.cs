using System.Threading.Tasks;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("Rename")]
    public class RenameChatCommand : AbstractCommand, IRenameChatCommand
    {
        private readonly IGameWebService _gameWebService;
        private readonly ITeamWebService _teamWebService;

        public RenameChatCommand(ILogger<IRenameChatCommand> logger, IGameWebService gameWebService, ITeamWebService teamWebService, IStringLocalizer<RenameChatCommand> localizer) : base(logger, localizer)
        {
            _gameWebService = gameWebService;
            _teamWebService = teamWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status == Status.None)
            {
                await turnContext.SendActivityAsync("Impossible de renommer le chat s'il n'est pas initialisé");
                return;
            }
            if (state.Game == null)
                state.Game = await _gameWebService.GetGameById(state.GameId.Value);
            if (state.Team == null)
                state.Team = await _teamWebService.GetTeamById(state.TeamId.Value);
            var activity = new Activity()
            {
                Type = ImageHuntActivityTypes.RenameChat,
                Text = $"Team {state.Team.Name} {state.Game.Name}",
            };
            await turnContext.SendActivityAsync(activity);
        }
    }
}