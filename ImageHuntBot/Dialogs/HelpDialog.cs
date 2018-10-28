using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class HelpDialog : AbstractDialog, IHelpDialog
    {
        public HelpDialog(ILogger<HelpDialog> logger) : base(logger)
        {
        }

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            LogInfo<ImageHuntState>(turnContext, "Begin HelpDialog");
            await turnContext.ReplyActivity(@"Pour jouer à la chasse au trésor, vous devez d'abord envoyer votre position. 
Le plus simples est de faire un partage en continu le temps que dure la chasse.
Ensuite, vous pouvez envoyer les images que vous aurez deviné, je les enregistrerai et un validateur vous attribuera les points si elle correspond à une des photo mystère!
Bon jeu!");
            await turnContext.End();
        }

        public override string Command => "/help";
    }
}
