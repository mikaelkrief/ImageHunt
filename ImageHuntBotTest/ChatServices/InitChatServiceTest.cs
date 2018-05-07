using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntBotTest.ChatServices;
using ImageHuntTelegramBot.Services;
using ImageHuntWebServiceClient.WebServices;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace ImageHuntBotTest
{
    public class InitChatServiceTest : ChatServiceBaseTest
    {
      private InitChatService _target;
      private ITelegramBotClient _client;
      private IGameWebService _gameWebService;
      private ITeamWebService _teamWebService;

      public InitChatServiceTest()
      {
        _client = A.Fake<ITelegramBotClient>();
        _gameWebService = A.Fake<IGameWebService>();
        _teamWebService = A.Fake<ITeamWebService>();
        _target = new InitChatService(_client, _gameWebService, _teamWebService, ChatPropertiesForChatId);
      }
      [Fact]
      public async Task Update()
      {
        // Arrange
        var message1 = new Message() {Text = "/init", Chat = new Chat() {Id = 15}};
        var message2 = new Message() {Text = "/game=15", Chat = new Chat() {Id = 15}};
        var message3 = new Message() {Text = "/team=16", Chat = new Chat() {Id = 15}};
        // Act
        await _target.Message(message1);
        Check.That(_target.Listen).IsTrue();
        await _target.Message(message2);
        Check.That(_target.Listen).IsTrue();
        await _target.Message(message3);
        Check.That(_target.Listen).IsFalse();
      // Assert
      A.CallTo(() =>
          _client.SendTextMessageAsync(A<ChatId>._, "", ParseMode.Default, false, false, 0, null,
            CancellationToken.None)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Times(3));
        A.CallTo(() => _gameWebService.GetGameById(15)).MustHaveHappened();
        A.CallTo(() => _teamWebService.GetTeamById(16)).MustHaveHappened();
        Check.That(_target.GameId).Equals(15);
        Check.That(_target.TeamId).Equals(16);
        Check.That(ChatPropertiesForChatId[15].GameId).Equals(15);
      }

      [Fact]
      public async Task Update_with_breaks()
      {
      // Arrange
        var message1 = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } };
        var message2 = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } };
        A.CallTo(() => _client.SendTextMessageAsync(A<ChatId>._, A<string>._, ParseMode.Default, false, false, 0, null,
          CancellationToken.None)).Returns(new Message() {Chat = new Chat() {Id = 15}, Text = "Merci de m'indiquer l'id de la partie : /game=id" });
      // Act
        await _target.Message(message1);
        await _target.Message(message2);

      // Assert
        A.CallTo(() =>
          _client.SendTextMessageAsync(A<ChatId>._, "Je n'ai pas compris votre dernière entrée, veuillez-recommencer :", 
            ParseMode.Default, false, false, 0, null,
            CancellationToken.None)).MustHaveHappened();
        A.CallTo(() =>
          _client.SendTextMessageAsync(A<ChatId>._, "Merci de m'indiquer l'id de la partie : /game=id", 
            ParseMode.Default, false, false, 0, null,
            CancellationToken.None)).MustHaveHappened(Repeated.Exactly.Twice);
    }

      [Fact]
      public async Task Update_with_number_bad_format()
      {
      // Arrange
        var message1 = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } ;
        var message2 = new Message() { Text = "/game=jghg", Chat = new Chat() { Id = 15 } };
        A.CallTo(() => _client.SendTextMessageAsync(A<ChatId>._, A<string>._, ParseMode.Default, false, false, 0, null,
          CancellationToken.None)).Returns(new Message() { Chat = new Chat() { Id = 15 }, Text = "Merci de m'indiquer l'id de la partie : /game=id" });
      // Act
      await _target.Message(message1);
        await _target.Message(message2);
      // Assert
        A.CallTo(() =>
          _client.SendTextMessageAsync(A<ChatId>._, "Je n'ai pas compris votre dernière entrée, veuillez-recommencer :",
            ParseMode.Default, false, false, 0, null,
            CancellationToken.None)).MustHaveHappened();
    }
  }
}
