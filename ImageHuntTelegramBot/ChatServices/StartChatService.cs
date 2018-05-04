using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Services;
using ImageHuntTelegramBot.WebServices;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.ChatServices
{
  public class StartChatService : AbstractChatService, IStartChatService
  {
    private readonly IGameWebService _gameWebService;
    private readonly ITeamWebService _teamWebService;

    public StartChatService(ITelegramBotClient client,
      IGameWebService gameWebService,
      ITeamWebService teamWebService, 
      Dictionary<long, ChatProperties> chatPropertiesForChatId) : base(client, chatPropertiesForChatId)
    {
      _client = client;
      _gameWebService = gameWebService;
      _teamWebService = teamWebService;
      Listen = true;
    }


    protected override async Task HandleMessage(Message message)
    {
      switch (message.Text)
      {
        case "/startgame":
          await SendTextMessageAsync(Chat.Id, "Vous allez démarrer la chasse, toute votre équipe est prête?");
          //_client.
          await _gameWebService.StartGameForTeam(CurrentChatProperties.GameId, CurrentChatProperties.TeamId);
          Listen = false;
          return;
      }
    }
  }
}