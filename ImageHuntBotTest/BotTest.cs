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
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "/init", ChatId = 15};
        A.CallTo(() => turnContext.Activity).Returns(activity);
        A.CallTo(() => turnContext.CurrentDialog).Returns(null);
        var initDialog = A.Fake<IDialog>();
        _target.AddDialog("/init", initDialog);
        // Act
        await _target.OnTurn(turnContext);
      // Assert
        A.CallTo(() => turnContext.Begin(initDialog)).MustHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();

    }
      [Fact]
      public async Task OnTurn_Uploadphoto()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "/uploadphoto", ChatId = 15};
        A.CallTo(() => turnContext.Activity).Returns(activity);
        A.CallTo(() => turnContext.CurrentDialog).Returns(null);
        var uploadPhotoDialog = A.Fake<IDialog>();
        _target.AddDialog("/uploadphoto", uploadPhotoDialog);
        // Act
        await _target.OnTurn(turnContext);
      // Assert
        A.CallTo(() => turnContext.Begin(uploadPhotoDialog)).MustHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();

    }
      [Fact]
      public async Task OnTurn_UploadDocument()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "/uploaddocument", ChatId = 15};
        A.CallTo(() => turnContext.Activity).Returns(activity);
        A.CallTo(() => turnContext.CurrentDialog).Returns(null);
        var uploadDocumentDialog = A.Fake<IDialog>();
        _target.AddDialog("/uploaddocument", uploadDocumentDialog);
        // Act
        await _target.OnTurn(turnContext);
      // Assert
        A.CallTo(() => turnContext.Begin(uploadDocumentDialog)).MustHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();

    }
    [Fact]
      public async Task OnTurn_DialogPending()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "15"};
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
