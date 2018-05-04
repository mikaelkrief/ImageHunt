using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Services
{
    public interface IChatService
    {
      bool Listen { get; set; }
      Task Message(Message message);
      Task CallbackQuery(CallbackQuery callbackQuery);
    }
}
