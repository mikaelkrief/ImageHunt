using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.ChatServices
{
    public class ChatProperties
    {
      public ChatProperties(ChatId chatId)
      {
        ChatId = chatId;
      }
      public ChatId ChatId { get; set; }
      public Message CurrentMessage { get; set; }

      public int GameId { get; set; }
      public int TeamId { get; set; }
    }
}
