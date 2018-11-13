using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("help")]
    public class HelpCommand : AbstractCommand, IHelpCommand
    {
        public HelpCommand(ILogger<IHelpCommand> logger) : base(logger)
        {
        }

        public override bool IsAdmin => false;
        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            await turnContext.SendActivityAsync(
                @"Pour jouer à la chasse au trésor, vous devez d'abord envoyer votre position. 
Le plus simples est de faire un partage en continu le temps que dure la chasse.
Ensuite, vous pouvez envoyer les images que vous aurez deviné, je les enregistrerai et un validateur vous attribuera les points si elle correspond à une des photo mystère!
Bon jeu!");
        }
    }
}