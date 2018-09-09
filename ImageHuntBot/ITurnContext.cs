using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntBot;

namespace ImageHuntTelegramBot
{
    public interface ITurnContext
    {
        IActivity Activity { get; set; }
        long ChatId { get; set; }
        string Username { get; set; }
        bool Replied { get; }
        IDialog CurrentDialog { get; }
        Task Reset();
        Task Begin(IDialog dialog);
        Task Continue();
        Task End();
        Task ReplyActivity(IActivity activity);
        Task ReplyActivity(string text);
        Task SendActivity(IActivity activity);
        event EventHandler EndCalled;
        T GetConversationState<T>() where T : IBaseState, new();
        Task ResetConversationStates<T>() where T : IBaseState, new();
        Task<IEnumerable<T>> GetAllConversationState<T>() where T : IBaseState, new();
    }
}
