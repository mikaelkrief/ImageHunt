using System;
using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
    public interface ITurnContext
    {
      IActivity Activity { get; set; }
      long ChatId { get; set; }
      bool Replied { get; }
      IDialog CurrentDialog { get; }

      Task Begin(IDialog dialog);
      Task Continue();
      Task End();
      Task ReplyActivity(IActivity activity);
      Task ReplyActivity(string text);
      Task SendActivity(IActivity activity);
      event EventHandler EndCalled;
      T GetConversationState<T>() where T : class, new();
        Task ResetConversationStates<T>() where T : class, new();
    }
}
