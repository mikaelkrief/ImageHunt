using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("help")]
    public class HelpCommand : AbstractCommand, IHelpCommand
    {
        public HelpCommand(ILogger<IHelpCommand> logger, IStringLocalizer<HelpCommand> localizer)
            : base(logger, localizer)
        {
        }

        public override bool IsAdmin => false;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            await turnContext.SendActivityAsync(
                Localizer["HELP"]);
        }
    }
}