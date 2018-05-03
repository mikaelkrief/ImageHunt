using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.ChatServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageHuntTelegramBot.Services
{
    public abstract class AbstractChatService : IChatService, IDisposable
    {
      protected static Dictionary<long, ChatProperties> _chatPropertiesForChatId = new Dictionary<long, ChatProperties>();
      protected ITelegramBotClient _client;

    public AbstractChatService(ITelegramBotClient client)
    {
      _client = client;
    }

    public ChatProperties this[long index]
      {
        get {
          try
          {
            return _chatPropertiesForChatId[index];
          }
          catch (Exception e)
          {
            
          }

          return null;
        }
      }
      public bool Listen { get; set; }

      public async Task Update(Update update)
      {
        if (update.Type != UpdateType.MessageUpdate)
          return;
        var message = update.Message;
        var chatId = message.Chat.Id;
        if (!_chatPropertiesForChatId.ContainsKey(chatId))
          _chatPropertiesForChatId.Add(chatId, new ChatProperties(chatId));
        await HandleMessage(message);
      }

      protected async Task<Message> SendTextMessageAsync(ChatId chatId, string text, ParseMode parseMode = ParseMode.Default,
        bool disableWebPagePreview = false, bool disableNotification = false, int replyToMessageId = 0,
        IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        var message = await _client.SendTextMessageAsync(chatId, text, parseMode,
          disableWebPagePreview, disableNotification, replyToMessageId,
          replyMarkup, cancellationToken);
        this[chatId.Identifier].CurrentMessage = message;
        return message;
      }

      protected async Task ResendMessage(ChatId chatId)
      {
        var currentMessage = this[chatId.Identifier].CurrentMessage;
        await _client.SendTextMessageAsync(currentMessage.Chat.Id, currentMessage.Text);
      }
      protected abstract Task HandleMessage(Message message);

      public void Dispose()
      {
      }
    }
}
