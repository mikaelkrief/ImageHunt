using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder.Commands.Interfaces
{
    public interface ICommand
    {
        bool IsAdmin { get; }

        Task ExecuteAsync(ITurnContext turnContext, ImageHuntState state);
    }
}