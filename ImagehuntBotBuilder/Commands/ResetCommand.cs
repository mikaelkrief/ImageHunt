using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("reset")]
    public class ResetCommand : AbstractCommand
    {
        public override bool IsAdmin => true;

        public ResetCommand(ILogger<IResetCommand> logger, IStringLocalizer<ResetCommand> localizer) : base(logger, localizer)
        {
        }

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            state.GameId = null;
            state.TeamId = null;
            state.CurrentLocation = null;
            state.Status = Status.None;
            await turnContext.SendActivityAsync($"Le groupe vient d'être ré-initialisé");
        }
    }
}