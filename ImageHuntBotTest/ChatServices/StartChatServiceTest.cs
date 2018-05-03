using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.ChatServices
{
    public class StartChatServiceTest : BaseTest
    {
      private StartChatService _target;
      private ITelegramBotClient _client;

      public StartChatServiceTest()
      {
        _client = A.Fake<ITelegramBotClient>();

      _target = new StartChatService(_client);
      }

      [Fact]
      public void Update()
      {
        // Arrange
        
        // Act

        // Assert
      }
    }

  public class StartChatService : AbstractChatService, IStartChatService
  {
    private ITelegramBotClient _client;

    public StartChatService(ITelegramBotClient client) : base(client)
    {
      _client = client;
    }

    protected override Task HandleMessage(Message message)
    {
      throw new NotImplementedException();
    }
  }
}
