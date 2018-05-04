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
      protected static Dictionary<long, ChatProperties> _chatPropertiesForChatId;
      protected ITelegramBotClient _client;

    public AbstractChatService(ITelegramBotClient client, 
      Dictionary<long, ChatProperties> chatPropertiesForChatId)
    {
      _client = client;
      _chatPropertiesForChatId = chatPropertiesForChatId;
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
      public Chat Chat { get; set; }
      public ChatProperties CurrentChatProperties => this[Chat.Id];

      public async Task Message(Message message)
      {
          Chat = message.Chat;
          var chatId = message.Chat.Id;
          if (!_chatPropertiesForChatId.ContainsKey(chatId))
            _chatPropertiesForChatId.Add(chatId, new ChatProperties(chatId));
          await HandleMessage(message);
      }

      public async Task CallbackQuery(CallbackQuery callbackQuery)
      {
        Chat = callbackQuery.Message.Chat;
        await HandleCallbackQuery(callbackQuery);
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

      //protected async Task<Message> SendChoice(ChatId chatId, string text, ParseMode parseMode = ParseMode.Default,
      //  bool disableWebPagePreview = false, bool disableNotification = false, int replyToMessageId = 0,
      //  IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
      //{

      //}
      protected async Task ResendMessage(ChatId chatId)
      {
        var currentMessage = this[chatId.Identifier].CurrentMessage;
        await _client.SendTextMessageAsync(currentMessage.Chat.Id, currentMessage.Text);
      }
      protected abstract Task HandleMessage(Message message);
      protected abstract Task HandleCallbackQuery(CallbackQuery callbackQuery);


      public void Dispose()
      {
      }

      protected async Task UnknownMessage()
      {
        await _client.SendTextMessageAsync(Chat.Id, "Je n'ai pas compris votre dernière entrée, veuillez-recommencer :");
        await ResendMessage(Chat.Id);
      }

      protected async Task<int> ExtractInt(string command, string text)
      {
        try
        {
          return Convert.ToInt32(text.Substring($"/{command}=".Length));
        }
        catch (FormatException e)
        {
          await UnknownMessage();
        }

        return 0;
      }
    }
}
