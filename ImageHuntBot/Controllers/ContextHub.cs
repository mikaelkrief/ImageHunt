using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Controllers
{
  public class ContextHub
  {
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IAdapter _adapter;
    private readonly Dictionary<long, ITurnContext> _turnContexts = new Dictionary<long, ITurnContext>();
    private static readonly object Padlock = new object();
    public ContextHub(ILifetimeScope lifetimeScope, IAdapter adapter)
    {
      _lifetimeScope = lifetimeScope;
      _adapter = adapter;
    }
    public virtual async Task<ITurnContext> GetContext(Update update)
    {
      long chatId = 0;
      string text = null;
      var activity = await _adapter.CreateActivityFromUpdate(update);
      //ActivityType activityType = ActivityType.None;
      //switch (update.Type)
      //{
      //  case UpdateType.MessageUpdate:
      //    chatId = update.Message.Chat.Id;
      //    text = update.Message.Text;
      //    activityType = ActivityType.Message;
      //    break;
      //  case UpdateType.CallbackQueryUpdate:
      //    chatId = update.CallbackQuery.Message.Chat.Id;
      //    activityType = ActivityType.CallbackQuery;
      //    break;
      //}
      chatId = activity.ChatId;
      lock (Padlock)
      {
        if (!_turnContexts.ContainsKey(chatId))
        {
          var turnContext = _lifetimeScope.Resolve<ITurnContext>();
          _turnContexts.Add(chatId, turnContext);
        }
        var tc = _turnContexts[chatId];
        tc.ChatId = chatId;
        tc.Activity = activity;

        return tc;
      }

    }
  }
}