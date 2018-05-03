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
    public class InitChatServiceTest
    {
      private InitChatService _target;
      private ITelegramBotClient _client;

      public InitChatServiceTest()
      {
        _client = A.Fake<ITelegramBotClient>();
        _target = new InitChatService(_client);
      }
      [Fact]
      public async Task Update()
      {
        // Arrange
        var update1 = new Update() {Message = new Message() {Text = "/init", Chat = new Chat() {Id = 15}}};
        var update2 = new Update() {Message = new Message() {Text = "/game=15", Chat = new Chat() {Id = 15}}};
        var update3 = new Update() {Message = new Message() {Text = "/team=16", Chat = new Chat() {Id = 15}}};
        // Act
        await _target.Update(update1);
        Check.That(_target.Listen).IsTrue();
        await _target.Update(update2);
        Check.That(_target.Listen).IsTrue();
        await _target.Update(update3);
        Check.That(_target.Listen).IsFalse();
      // Assert
      A.CallTo(() =>
          _client.SendTextMessageAsync(A<ChatId>._, "", ParseMode.Default, false, false, 0, null,
            CancellationToken.None)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Times(3));
        Check.That(_target.GameId).Equals(15);
        Check.That(_target.TeamId).Equals(16);
      }
    }
}
