using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("reset")]
    public class ResetCommand : AbstractCommand
    {
        public override bool IsAdmin => true;

        public ResetCommand(ILogger<IResetCommand> logger) : base(logger)
        {
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            state.GameId = null;
            state.TeamId = null;
            state.CurrentLocation = null;
            state.Status = Status.None;
            await turnContext.SendActivityAsync($"Le groupe vient d'être ré-initialisé");
        }
    }
}