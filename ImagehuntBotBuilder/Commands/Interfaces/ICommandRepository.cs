using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder.Commands.Interfaces
{
    public interface ICommandRepository
    {
        ICommand Get(ITurnContext turnContext, ImageHuntState state, string commandText);
        Task RefreshAdmins();
    }
}