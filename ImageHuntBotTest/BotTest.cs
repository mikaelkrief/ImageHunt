using System;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class BotTest : BaseTest
    {
      private TelegramBot _target;

      public BotTest()
      {
        _testContainerBuilder.RegisterType<TelegramBot>();
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<TelegramBot>();
      }

      [Fact]
      public async Task OnTurn_NoDialog()
      {
      // Arrange
        var turnContext = A.Fake<ITurnContext>();
        // Act
        await _target.OnTurn(turnContext);
        // Assert
      }
      [Fact]
      public async Task OnTurn_Init()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = A.Fake<IActivity>();
        A.CallTo(() => activity.ActivityType).Returns(ActivityType.Message);
        A.CallTo(() => activity.Text).Returns("/init");
        A.CallTo(() => turnContext.Activity).Returns(activity);
        var initDialog = A.Fake<IDialog>();
        _target.AddDialog("/init", initDialog);
        // Act
        await _target.OnTurn(turnContext);
      // Assert
        A.CallTo(() => initDialog.Begin(turnContext)).MustHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();

    }
    [Fact]
      public async Task OnTurn_DialogPending()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = A.Fake<IActivity>();
        A.CallTo(() => activity.ActivityType).Returns(ActivityType.Message);
        A.CallTo(() => activity.Text).Returns("/init");
        A.CallTo(() => turnContext.Activity).Returns(activity);
        A.CallTo(() => turnContext.Replied).Returns(true);
        var initDialog = A.Fake<IDialog>();
        _target.AddDialog("/init", initDialog);
        // Act
        await _target.OnTurn(turnContext);
        // Assert
        A.CallTo(() => initDialog.Begin(turnContext)).MustNotHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();
      }
  }
}
