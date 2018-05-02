using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
    public class UpdateService : IUpdateService
    {
      private readonly BotConfiguration _config;
      private readonly ITelegramBotClient _client;

      public UpdateService(IOptions<BotConfiguration> config)
      {
        _config = config.Value;
        _client = new TelegramBotClient(_config.BotToken);
      }
      public async Task Root(Update update)
      {
        if (update.Type != UpdateType.MessageUpdate)
          return;
        var message = update.Message;
        switch (message.Text)
        {
        case "/init":
          break;
        default:
          await _client.SendTextMessageAsync(message.Chat.Id,
            "Je n'ai pas compris votre demande, veuillez vous en tenir aux commandes standard");
          break;
        }
      }
    }
}
