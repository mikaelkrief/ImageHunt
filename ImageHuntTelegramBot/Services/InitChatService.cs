using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
    public class InitChatService : AbstractChatService, IInitChatService
    {
      private readonly ITelegramBotClient _client;

      public InitChatService(ITelegramBotClient client)
      {
        _client = client;
      Listen = true;
      }

      protected override async Task HandleMessage(Message message)
      {
        switch (message.Text)
        {
        case "/init":
          Chat = message.Chat;
          await _client.SendTextMessageAsync(Chat.Id, "Merci de m'indiquer l'id de la partie : /game=id");
          return;
        case var s when s.StartsWith("/game"):
          GameId = Convert.ToInt32(s.Substring("/game=".Length));
          await _client.SendTextMessageAsync(Chat.Id, $"Vous participez à la partie {GameId}. Merci de m'indiquer l'id de l'équipe : /team=id");
          return;
        case var s when s.StartsWith("/team"):
          TeamId = Convert.ToInt32(s.Substring("/team=".Length));
          await _client.SendTextMessageAsync(Chat.Id, $"Ce chat est celui de l'équipe {TeamId}. Merci, le chat est prêt, bonne partie!");
          // Stop listen the chat
          Listen = false;
          return;

      }

    }

      public int TeamId { get; set; }

      public int GameId { get; set; }

      public Chat Chat { get; set; }
    }
}
