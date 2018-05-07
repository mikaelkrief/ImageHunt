using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntWebServiceClient.WebServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
    public class InitChatService : AbstractChatService, IInitChatService
    {
      private readonly IGameWebService _gameWebService;
      private readonly ITeamWebService _teamWebService;
      public int TeamId { get; set; }

      public int GameId { get; set; }

      public InitChatService(ITelegramBotClient client, 
        IGameWebService gameWebService, 
        ITeamWebService teamWebService, Dictionary<long, ChatProperties> chatPropertiesForChatId) : base(client, chatPropertiesForChatId)
      {
        _gameWebService = gameWebService;
        _teamWebService = teamWebService;
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
          GameId = await ExtractInt("game", s);
          this[Chat.Id].GameId = GameId;
          var game = this[Chat.Id].Game = await _gameWebService.GetGameById(GameId);
          await SendTextMessageAsync(Chat.Id, $"Vous participez à la partie {game.Name} qui débutera {game.StartDate}. Merci de m'indiquer l'id de l'équipe : /team=id");
          return;
        case var s when s.StartsWith("/team"):
          TeamId = await ExtractInt("team", s);
          this[Chat.Id].TeamId = TeamId;

          var team = this[Chat.Id].Team = await _teamWebService.GetTeamById(TeamId);
          await SendTextMessageAsync(Chat.Id, $"Ce chat est celui de l'équipe {team.Name}. Merci, le chat est prêt, bonne partie!");
          await _client.SetChatTitleAsync(Chat.Id,
            $"Groupe de l'équipe {team.Name} pour la chasse {this[Chat.Id].Game.Name}");
          // Stop listen the chat
          Listen = false;
          return;
        default:
          await UnknownMessage();
          break;

      }

    }

      protected override Task HandleCallbackQuery(CallbackQuery callbackQuery)
      {
        throw new NotImplementedException();
      }
    }
}
