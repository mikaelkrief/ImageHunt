using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public class TurnContext : ITurnContext
  {
    private readonly IAdapter _adapter;
    public virtual IActivity Activity { get; set; }
    public virtual long ChatId { get; set; }
    public virtual bool Replied { get; private set; }
    public virtual IDialog CurrentDialog { get; private set; }

    public TurnContext(IAdapter adapter)
    {
      _adapter = adapter;
    }
    private static readonly Dictionary<long, object> ConversationStates = new Dictionary<long, object>();
    public virtual T GetConversationState<T>() where T : class, new()
    {
      if (!ConversationStates.ContainsKey(ChatId))
      {
        ConversationStates.Add(ChatId, new T());
      }

      return (T)ConversationStates[ChatId];
    }

    public virtual async Task Continue()
    {
      if (CurrentDialog != null)
        await CurrentDialog.Continue(this);
    }

    public virtual async Task End()
    {
      EndCalled?.Invoke(this, new EventArgs());
      Replied = false;
      CurrentDialog = null;
    }

    public virtual async Task ReplyActivity(IActivity activity)
    {
      Replied = true;
      await _adapter.SendActivity(activity);
    }

    public virtual async Task ReplyActivity(string text)
    {
      var activity = new Activity(){ActivityType = ActivityType.Message, ChatId = this.ChatId, Text = text};

      await ReplyActivity(activity);
    }

    public virtual async Task SendActivity(IActivity activity)
    {
      await _adapter.SendActivity(activity);
    }

    public event EventHandler EndCalled;

    public virtual async Task Begin(IDialog dialog)
    {
      CurrentDialog = dialog;
      await CurrentDialog.Begin(this);
    }
  }
}