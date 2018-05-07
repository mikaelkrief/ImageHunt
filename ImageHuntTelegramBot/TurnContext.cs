using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public class TurnContext : ITurnContext
  {
    public IActivity Activity { get; set; }
    public string ChatId { get; set; }
    public bool Replied { get; private set; }
    public IDialog CurrentDialog { get; set; }

    private static readonly Dictionary<string, object> ConversationStates = new Dictionary<string, object>();
    public T GetConversationState<T>() where T : class, new()
    {
      if (!ConversationStates.ContainsKey(ChatId))
      {
        ConversationStates.Add(ChatId, new T());
      }

      return (T)ConversationStates[ChatId];
    }

    public async Task Continue()
    {
      if (CurrentDialog != null)
        await CurrentDialog.Continue(this);
    }

    public async Task End()
    {
      CurrentDialog = null;
    }

    public async Task ReplyActivity(IActivity activity)
    {
      Replied = true;
    }

    public async Task Begin(IDialog dialog)
    {
      CurrentDialog = dialog;
      await CurrentDialog.Begin(this);
    }
  }
}