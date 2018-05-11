using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Services;
using ImageHuntWebServiceClient.WebServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

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
      _gameWebService = gameWebService;
      _teamWebService = teamWebService;
      Listen = true;
    }


    protected override async Task HandleMessage(Message message)
    {
      switch (message.Text)
      {
        case "/startgame":
          var choiceReplyMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {new InlineKeyboardCallbackButton("Oui", "Yes"), new InlineKeyboardCallbackButton("Non", "No")});
          await SendTextMessageAsync(Chat.Id,
             "Vous allez démarrer la chasse, toute votre équipe est prête?",
             replyMarkup: choiceReplyMarkup);
          return;
      }
    }

    protected override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
      switch (callbackQuery.Data)
      {
        case "Yes":
          var nodeResponse = await _gameWebService.StartGameForTeam(CurrentChatProperties.GameId, CurrentChatProperties.TeamId);
          await SendTextMessageAsync(Chat.Id, "Veuillez vous rendre au point de départ");
          await _client.SendLocationAsync(Chat.Id, (float) nodeResponse.Latitude, (float) nodeResponse.Longitude);
          Listen = false;
          break;
        case "No":
          await SendTextMessageAsync(Chat.Id, "Attendez d'être au complet et nous pourrons débuter");
          break;
      }
    }
  }
}