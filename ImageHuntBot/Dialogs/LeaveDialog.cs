using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class LeaveDialog : AbstractDialog, ILeaveDialog
    {
        public LeaveDialog(ILogger<LeaveDialog> logger) : base(logger)
        {
        }

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            try
            {
                await turnContext.ReplyActivity($"Mon travail ici est terminé, je m'en vais");
                await turnContext.Leave();
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/leave";
    }
}