using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder
{
    [Command("end")]
    public class EndCommand : AbstractCommand, IEndCommand
    {
        private readonly IActionWebService _actionWebService;

        public EndCommand(ILogger<IEndCommand> logger, IActionWebService actionWebService) : base(logger)
        {
            _actionWebService = actionWebService;
        }

        public override bool IsAdmin { get; }
        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    "La partie n'a pas encore commencée, vous ne pouvez par l'arrêter!");
                _logger.LogError("The game had not started!");
                return;
            }
            var gameActionRequest = new GameActionRequest()
            {
                GameId = state.GameId.Value,
                TeamId = state.TeamId.Value,
                Action = (int)ImageHuntWebServiceClient.Action.EndGame,

            };
            await _actionWebService.LogAction(gameActionRequest);
            state.Status = Status.Ended;
            await turnContext.SendActivityAsync(
                "La chasse vient de prendre fin, vos actions ont été enregistrée et un orga va les valider.");
        }
    }
}