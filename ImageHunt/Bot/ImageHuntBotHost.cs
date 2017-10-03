using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageHunt.Services;
using ImageHuntBot;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHunt.Bot
{
    public class ImageHuntBotHost : BotHost
    {
      private readonly HttpClient _httpClient;

      public ImageHuntBotHost(IConfiguration configuration,
                              ITelegramBotClient telegramBotClient,
                              HttpClient httpClient)
      : base(configuration, telegramBotClient)
      {
        _httpClient = httpClient;
      }

      protected override async Task OnPhotoMessage(Message message)
      {
        var user = message.From;
        // Call the webservice to get the user

      }

    protected override async Task OnTextMessage(Message message)
      {
        await _bot.SendTextMessageAsync(message.Chat.Id, $"Echo {message.Text}");
      }
    }
}
