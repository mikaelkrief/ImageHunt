using System;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntTelegramBot.WebServices;
using Telegram.Bot;
using Telegram.Bot.Types;
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
      public async Task Update()
      {
        // Arrange
        var update = new Update(){Message = new Message(){Chat = new Chat(){Id = 15}, Text = "/startgame"}};
        // Act
        await _target.Update(update);
        // Assert
        A.CallTo(() => _gameWebService.StartGameForTeam(A<int>._, A<int>._)).MustHaveHappened();
      }
    }
}
