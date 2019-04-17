using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotCore.Commands.Interfaces
{
    public interface ICommandRepository<TState>
    where TState : IState
    {
        ICommand<TState> Get(ITurnContext turnContext, TState state, string commandText);
        Task RefreshAdminsAsync();
    }
}