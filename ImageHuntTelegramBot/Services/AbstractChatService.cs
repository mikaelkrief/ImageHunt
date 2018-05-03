using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
    public abstract class AbstractChatService : IChatService
    {
      public bool Listen { get; set; }

      public async Task Update(Update update)
      {
        if (update.Type != UpdateType.MessageUpdate)
          return;
        var message = update.Message;
        await HandleMessage(message);
      }

      protected abstract Task HandleMessage(Message message);
    }
}
