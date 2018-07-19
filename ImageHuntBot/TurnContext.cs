using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot
{
  public class TurnContext : ITurnContext
  {
    private readonly IAdapter _adapter;
    private readonly IStorage _storage;
    private readonly ILogger<TurnContext> _logger;
    public virtual IActivity Activity { get; set; }
    public virtual long ChatId { get; set; }
    public virtual bool Replied { get; private set; }
    public virtual IDialog CurrentDialog { get; private set; }

    public TurnContext(IAdapter adapter, IStorage storage, ILogger<TurnContext> logger)
    {
      _adapter = adapter;
      _storage = storage;
      _logger = logger;
    }
    private static readonly Dictionary<long, object> ConversationStates = new Dictionary<long, object>();

    public void ResetConversationStates()
    {
      ConversationStates.Remove(ChatId);
    }
    private readonly object padlock = new object();
    public virtual T GetConversationState<T>() where T : class, new()
    {
      lock (padlock)
      {
        if (!ConversationStates.ContainsKey(ChatId))
        {
          ConversationStates.Add(ChatId, new T());
        }

        return (T) ConversationStates[ChatId];
      }
    }

    public virtual async Task Continue()
    {
      if (CurrentDialog != null)
        await CurrentDialog.Continue(this);
    }

    public virtual async Task End()
    {
      EndCalled?.Invoke(this, new EventArgs());
      if (ConversationStates.ContainsKey(ChatId))
        await _storage.Write(new[] {new KeyValuePair<string, object>(ChatId.ToString(), ConversationStates[ChatId])});
      Replied = false;
      CurrentDialog = null;
    }

    public virtual async Task ReplyActivity(IActivity activity)
    {
      Replied = true;
      try
      {
        await _adapter.SendActivity(activity);

      }
      catch (Exception e)
      {
        _logger.LogError(e, "Error while replying");
      }
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
      if (ChatId != 0)
      {
        if (!ConversationStates.ContainsKey(ChatId))
        {
          var state = await _storage.Read(ChatId.ToString());
          if (state.Any())
            ConversationStates.Add(ChatId, state.First().Value);
        }
      }

      try
      {
        await CurrentDialog.Begin(this);

      }
      catch (Exception e)
      {
        _logger.LogError(e, $"Error while begin dialog {CurrentDialog}");
      }
    }
  }
}