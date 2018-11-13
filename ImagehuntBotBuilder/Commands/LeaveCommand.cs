using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("leave")]
    public class LeaveCommand : AbstractCommand, ILeaveCommand
    {
        public LeaveCommand(ILogger<ILeaveCommand> logger) : base(logger)
        {
        }

        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            var activity = new Activity(type: ImageHuntActivityTypes.Leave);
            await turnContext.SendActivityAsync(activity);
        }
    }
}