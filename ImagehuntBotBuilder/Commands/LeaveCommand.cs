using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("leave")]
    public class LeaveCommand : AbstractCommand, ILeaveCommand
    {
        public LeaveCommand(ILogger<ILeaveCommand> logger, IStringLocalizer<LeaveCommand> localizer) : base(logger, localizer)
        {
        }

        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            var activity = new Activity(type: ImageHuntActivityTypes.Leave);
            await turnContext.SendActivityAsync(activity);
        }
    }
}