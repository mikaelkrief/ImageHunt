using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class UpdateControllerTest : BaseTest
    {
      private UpdateController _target;
      private IBot _bot;
      private ContextHub _contextHub;
      private ILogger _logger;

      public UpdateControllerTest()
      {
        _testContainerBuilder.RegisterType<UpdateController>();
        _bot = A.Fake<IBot>();
        _logger = A.Fake<ILogger<UpdateController>>();
        _testContainerBuilder.RegisterInstance(_logger).As<ILogger<UpdateController>>();
        _testContainerBuilder.RegisterInstance(_bot);
        _contextHub = A.Fake<ContextHub>();
        _testContainerBuilder.RegisterInstance(_contextHub).SingleInstance();
        _container  = _testContainerBuilder.Build();
        _target = _container.Resolve<UpdateController>();
      }
      [Fact]
      public async Task Update_Message()
      {
        // Arrange
        var update = new Update() {Message = new Message() {Chat = new Chat() {Id = 15}}};
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened();
      }
      [Fact]
      public async Task Update_UpdateMessage()
      {
        // Arrange
        var update = new Update() {EditedMessage = new Message() {Chat = new Chat() {Id = 15}}};
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened();
      }
      [Fact]
      public async Task Update_Message_Exception_Occured()
      {
        // Arrange
        var update = new Update() {Message = new Message() {Chat = new Chat() {Id = 15}}};
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).Throws<Exception>();
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened(Repeated.Exactly.Twice);
      }
    }
}
