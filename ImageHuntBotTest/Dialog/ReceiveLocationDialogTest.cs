using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class ReceiveLocationDialogTest : BaseTest
    {
      private IReceiveLocationDialog _target;
      private ILogger _logger;

      public ReceiveLocationDialogTest()
      {
        _testContainerBuilder.RegisterType<ReceiveLocationDialog>().As<IReceiveLocationDialog>();
        _logger = A.Fake<ILogger>();
        _testContainerBuilder.RegisterInstance(_logger);

      _container = _testContainerBuilder.Build();
        _target = _container.Resolve<IReceiveLocationDialog>();
      }

      [Fact]
      public async Task Begin()
      {
        // Arrange
        var activity = new Activity()
        {
          ActivityType = ActivityType.Message,
          ChatId = 15,
          Location = new Location() {Latitude = 15.6f, Longitude = 4.2f}
        };
        var turnContext = A.Fake<ITurnContext>();
        A.CallTo(() => turnContext.Activity).Returns(activity);
        var imageHuntState = new ImageHuntState();
        A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      // Act
      await _target.Begin(turnContext);
        // Assert
        A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
        Check.That(imageHuntState.CurrentLatitude).Equals(15.6f);
        Check.That(imageHuntState.CurrentLongitude).Equals(4.2f);
        A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
            A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened();
      }
  }
}
