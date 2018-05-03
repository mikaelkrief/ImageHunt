using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntTelegramBot.WebServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
    public class InitChatService : AbstractChatService, IInitChatService
    {
      private readonly IGameWebService _gameWebService;
      
      public InitChatService(ITelegramBotClient client, IGameWebService gameWebService) : base(client)
      {
        _gameWebService = gameWebService;
        Listen = true;
      }

      protected override async Task HandleMessage(Message message)
      {
          Chat = message.Chat;
        switch (message.Text)
        {
        case "/init":
          await SendTextMessageAsync(Chat.Id, "Merci de m'indiquer l'id de la partie : /game=id");
          return;
        case var s when s.StartsWith("/game"):
          GameId = Convert.ToInt32(s.Substring("/game=".Length));
          var game = await _gameWebService.GetGameById(GameId);
          await SendTextMessageAsync(Chat.Id, $"Vous participez à la partie {game.Name} qui débutera {game.StartDate}. Merci de m'indiquer l'id de l'équipe : /team=id");
          return;
        case var s when s.StartsWith("/team"):
          TeamId = Convert.ToInt32(s.Substring("/team=".Length));
          await SendTextMessageAsync(Chat.Id, $"Ce chat est celui de l'équipe {TeamId}. Merci, le chat est prêt, bonne partie!");
          // Stop listen the chat
          Listen = false;
          return;
        default:
          await _client.SendTextMessageAsync(Chat.Id, "Je n'ai pas compris votre dernière entrée, veuillez-recommencer :");
          await ResendMessage(Chat.Id);
          break;

      }

    }

      public int TeamId { get; set; }

      public int GameId { get; set; }

      public Chat Chat { get; set; }
    }
}
