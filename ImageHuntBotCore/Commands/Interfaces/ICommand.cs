using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotCore.Commands.Interfaces
{
    public interface ICommand<TState>
        where TState : IState
    {
        bool IsAdmin { get; }
        Task ExecuteAsync(ITurnContext turnContext, TState state);
    }
}