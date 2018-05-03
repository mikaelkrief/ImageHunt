using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Services
{
    public class DefaultChatService : AbstractChatService, IDefaultChatService
    {
      private readonly ITelegramBotClient _telegramClient;

      public DefaultChatService(ITelegramBotClient telegramClient) : base(telegramClient)
      {
        _telegramClient = telegramClient;
      }


      protected override async Task HandleMessage(Message message)
      {
        await _telegramClient.SendTextMessageAsync(message.Chat.Id,
          "Je n'ai pas compris votre demande, veuillez vous en tenir aux commandes standard");
      }
    }
}
