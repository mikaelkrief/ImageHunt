using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHuntBot.Dialogs
{
    public class ResetDialog : AbstractDialog, IResetDialog
    {
        public ResetDialog(ILogger<ResetDialog> logger) : base(logger)
        {
        }
        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            LogInfo<ImageHuntState>(turnContext, "Reset Game");
            await turnContext.ResetConversationStates<ImageHuntState>();
            await turnContext.ReplyActivity($"Le groupe {turnContext.ChatId} vient d'être ré-initialisé");
            await turnContext.End();
        }

        public override string Command => "/reset";
    }
}
