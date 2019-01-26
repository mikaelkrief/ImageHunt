using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("end")]
    public class EndCommand : AbstractCommand, IEndCommand
    {
        private readonly IActionWebService _actionWebService;

        public EndCommand(ILogger<IEndCommand> logger, IActionWebService actionWebService, IStringLocalizer<EndCommand> localizer) : base(logger, localizer)
        {
            _actionWebService = actionWebService;
        }

        public override bool IsAdmin => true;
        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    _localizer["CANNOT_END_GAME_NOT_STARTED"]);
                _logger.LogError("The game had not started!");
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
            await turnContext.SendActivityAsync(_localizer["GAME_ENDED"]);
            _logger.LogInformation($"Game {state.GameId} ended for team {state.TeamId}");
        }
    }
}