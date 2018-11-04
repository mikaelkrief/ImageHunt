using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder.Commands
{
    public interface ICommandRepository
    {
        ICommand Get(ITurnContext turnContext, string commandText);
    }
}