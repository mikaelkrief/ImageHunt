using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class NewUserDialog : AbstractDialog, INewUserDialog
    {
        public NewUserDialog(ILogger<NewUserDialog> logger) : base(logger)
        {
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            try
            {
                if (state.GameId == 0 || state.TeamId == 0)
                {
                    _logger.LogError($"Unable to add a user to a team for a non-initialized group");
                }
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/newUser";
    }
}