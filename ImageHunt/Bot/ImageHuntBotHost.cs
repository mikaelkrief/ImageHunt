using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntBot;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;

namespace ImageHunt.Bot
{
    public class ImageHuntBotHost : BotHost
    {
      public ImageHuntBotHost(IConfiguration configuration) : base(configuration)
      {
      }

      public override async Task OnTextMessage(Message message)
      {
        await _bot.SendTextMessageAsync(message.Chat.Id, $"Echo {message.Text}");
      }
    }
}
