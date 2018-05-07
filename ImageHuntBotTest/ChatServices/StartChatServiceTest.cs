using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace ImageHuntBotTest.ChatServices
{
  public class StartChatServiceTest : ChatServiceBaseTest
  {
    private StartChatService _target;
    private ITelegramBotClient _client;
    private ITeamWebService _teamWebService;
    private IGameWebService _gameWebService;

    public StartChatServiceTest()
    {
      _client = A.Fake<ITelegramBotClient>();
      _teamWebService = A.Fake<ITeamWebService>();
      _gameWebService = A.Fake<IGameWebService>();
      _target = new StartChatService(_client, _gameWebService, _teamWebService, ChatPropertiesForChatId);
    }

    [Fact]
    public async Task Message()
    {
      // Arrange
      var message = new Message() { Chat = new Chat() { Id = 15 }, Text = "/startgame" };
      // Act
      await _target.Message(message);
      // Assert
      A.CallTo(() => _client.SendTextMessageAsync(A<ChatId>._, A<string>._, A<ParseMode>._, A<bool>._,
        A<bool>._, A<int>._, A<InlineKeyboardMarkup>.That.Matches(m => CheckMarkup(m)), A<CancellationToken>._)).MustHaveHappened();
    }

    private bool CheckMarkup(InlineKeyboardMarkup inlineKeyboardMarkup)
    {
      var keyboard = inlineKeyboardMarkup.InlineKeyboard;
      Check.That(keyboard[0][0]).IsInstanceOf<InlineKeyboardCallbackButton>();
      return true;
    }

    [Fact]
    public async Task CallbackQuery()
    {
      // Arrange
      var message = new Message() { Chat = new Chat() { Id = 15 }, Text = "/startgame" };
      var callbackQuery = new CallbackQuery() { Data = "Yes", Message = message };
      // Act
      await _target.Message(message);
      await _target.CallbackQuery(callbackQuery);
      // Assert
      A.CallTo(() => _gameWebService.StartGameForTeam(A<int>._, A<int>._)).MustHaveHappened();
    }
    [Fact]
    public async Task CallbackQuery_UserReplied_Yes()
    {
      // Arrange
      var message = new Message() { Chat = new Chat() { Id = 15 }, Text = "/startgame" };
      var callbackQuery = new CallbackQuery() { Data = "Yes", Message = message };
      var nodeResponse = new NodeResponse(){Latitude = 15.5, Longitude = 14.6, Name = "Départ"};
      A.CallTo(() => _gameWebService.StartGameForTeam(A<int>._, A<int>._))
        .Returns(Task.FromResult(nodeResponse));
      // Act
      await _target.Message(message);
      await _target.CallbackQuery(callbackQuery);
      // Assert
      A.CallTo(() => _gameWebService.StartGameForTeam(A<int>._, A<int>._)).MustHaveHappened();
      A.CallTo(() => _client.SendLocationAsync(A<ChatId>._, (float) nodeResponse.Latitude,
          (float) nodeResponse.Longitude, A<int>._, A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._))
        .MustHaveHappened();
    }
    [Fact]
    public async Task CallbackQuery_UserReplied_No()
    {
      // Arrange
      var message = new Message() { Chat = new Chat() { Id = 15 }, Text = "/startgame" };
      var callbackQuery = new CallbackQuery() { Data = "No", Message = message };
      // Act
      await _target.Message(message);
      await _target.CallbackQuery(callbackQuery);
      // Assert
      A.CallTo(() => _gameWebService.StartGameForTeam(A<int>._, A<int>._)).MustNotHaveHappened();
    }
  }
}
