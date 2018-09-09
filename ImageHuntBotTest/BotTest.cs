using System;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class BotTest : BaseTest
    {
      private TelegramBot _target;
      private ILogger<TelegramBot> _logger;

      public BotTest()
      {
        _logger = A.Fake<ILogger<TelegramBot>>();
        _testContainerBuilder.RegisterInstance(_logger);
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
          A.CallTo(() => initDialog.Command).Returns("/init");
        _target.AddDialog(initDialog);
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
          A.CallTo(() => uploadPhotoDialog.Command).Returns("/uploadphoto");

            _target.AddDialog(uploadPhotoDialog);
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
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/uploaddocument", ChatId = 15 };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            var uploadDocumentDialog = A.Fake<IDialog>();
            A.CallTo(() => uploadDocumentDialog.Command).Returns("/uploaddocument");
            _target.AddDialog(uploadDocumentDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(uploadDocumentDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }
        [Fact]
        public async Task OnTurn_ResetBot()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/reset", ChatId = 15 };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            var resetDialog = A.Fake<IDialog>();
            A.CallTo(() => resetDialog.Command).Returns("/reset");
            _target.AddDialog(resetDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(resetDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }

        [Fact]
      public async Task OnTurn_ErrorOccured()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "/uploaddocument", ChatId = 15};
        A.CallTo(() => turnContext.Activity).Returns(activity);
        A.CallTo(() => turnContext.CurrentDialog).Returns(null);
        A.CallTo(() => turnContext.Begin(A<IDialog>._)).Throws<Exception>();
        var uploadDocumentDialog = A.Fake<IDialog>();
          A.CallTo(() => uploadDocumentDialog.Command).Returns("/uploaddocument");
       _target.AddDialog(uploadDocumentDialog);
            // Act
          await _target.OnTurn(turnContext);
      // Assert
          A.CallTo(() => turnContext.End()).MustHaveHappened();

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
          A.CallTo(() => initDialog.Command).Returns("/init");
        _target.AddDialog(initDialog);
        // Act
        await _target.OnTurn(turnContext);
        // Assert
        A.CallTo(() => initDialog.Begin(turnContext)).MustNotHaveHappened();
        A.CallTo(() => turnContext.Continue()).MustHaveHappened();
      }
      
  }
}
