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
        protected ResetDialog(ILogger logger) : base(logger)
        {
        }
        public override async Task Begin(ITurnContext turnContext)
        {
            _logger.LogInformation($"Reset the bot for chatId {turnContext.ChatId}");
            await turnContext.ResetConversationStates<ImageHuntState>();
            await turnContext.ReplyActivity($"Le groupe {turnContext.ChatId} vient d'être ré-initialisé");
        }
    }
}
