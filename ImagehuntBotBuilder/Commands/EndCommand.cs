using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore.Commands;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("end")]
    public class EndCommand : AbstractCommand<ImageHuntState>, IEndCommand
    {
        private readonly IActionWebService _actionWebService;

        public EndCommand(ILogger<IEndCommand> logger, IActionWebService actionWebService, IStringLocalizer<EndCommand> localizer) : base(logger, localizer)
        {
            _actionWebService = actionWebService;
        }

        public override bool IsAdmin => true;
        protected async override Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    Localizer["CANNOT_END_GAME_NOT_STARTED"]);
                Logger.LogError("The game had not started!");
                return;
            }
            var gameActionRequest = new GameActionRequest()
            {
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Action = (int)Action.EndGame,

            };
            await _actionWebService.LogAction(gameActionRequest);
            state.Status = Status.Ended;
            await turnContext.SendActivityAsync(Localizer["GAME_ENDED"]);
            Logger.LogInformation("Game {0} ended for team {1}", state.GameId, state.TeamId);
        }
    }
}