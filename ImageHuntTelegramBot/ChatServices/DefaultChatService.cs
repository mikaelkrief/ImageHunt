using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntTelegramBot.ChatServices;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Services
{
    public class DefaultChatService : AbstractChatService, IDefaultChatService
    {

      public DefaultChatService(ITelegramBotClient telegramClient, 
        Dictionary<long, ChatProperties> chatPropertiesForChatId) 
        : base(telegramClient, chatPropertiesForChatId)
      {
      }


      protected override async Task HandleMessage(Message message)
      {
        await SendTextMessageAsync(message.Chat.Id,
          "Je n'ai pas compris votre demande, veuillez vous en tenir aux commandes standard");
      }

      protected override Task HandleCallbackQuery(CallbackQuery callbackQuery)
      {
        throw new NotImplementedException();
      }
    }
}
