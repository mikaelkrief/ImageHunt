using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.Services;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace ImageHuntBotTest
{
    public class DefaultChatServiceTest
    {
      private DefaultChatService _target;
      private ITelegramBotClient _telegramClient;

      public DefaultChatServiceTest()
      {
        _telegramClient = A.Fake<ITelegramBotClient>();
        _target = new DefaultChatService(_telegramClient);
      }
      [Fact]
      public async Task Update()
      {
        // Arrange
        var update = new Update(){Message = new Message(){Chat = new Chat(){Id = 15}}};
        // Act
        await _target.Update(update);
        // Assert
        Check.That(_target.Listen).IsFalse();
        A.CallTo(() => _telegramClient.SendTextMessageAsync(A<ChatId>._, "", ParseMode.Default, false, false, 0, null,
          CancellationToken.None)).WithAnyArguments().MustHaveHappened();
      }
    }
}
