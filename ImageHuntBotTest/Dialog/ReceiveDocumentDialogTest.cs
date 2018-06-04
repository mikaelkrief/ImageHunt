using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;
using File = Telegram.Bot.Types.File;

namespace ImageHuntBotTest
{
  public class ReceiveDocumentDialogTest : BaseTest
  {
    private ReceiveDocumentDialog _target;
    private ITeamWebService _teamWebService;
    private ITelegramBotClient _telegramBotClient;
    private ILogger _logger;

    public ReceiveDocumentDialogTest()
    {
      _testContainerBuilder.RegisterType<ReceiveDocumentDialog>();
      _logger = A.Fake<ILogger>();
      _testContainerBuilder.RegisterInstance(_logger);
      _teamWebService = A.Fake<ITeamWebService>();
      _telegramBotClient = A.Fake<ITelegramBotClient>();
      _testContainerBuilder.RegisterInstance(_telegramBotClient).As<ITelegramBotClient>();

      _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
      _container = _testContainerBuilder.Build();
      _target = _container.Resolve<ReceiveDocumentDialog>();
    }

    [Fact]
    public async Task Begin()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { GameId = 15, TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      A.CallTo(() => _telegramBotClient.GetInfoAndDownloadFileAsync(A<string>._, A<Stream>._, A<CancellationToken>._))
        .Invokes((string fileId, Stream stream, CancellationToken cancellationToken) => { stream.Write(new byte[10], 0, 10);});
      var document1 = new Document(){FileSize = 15, FileId = "15", MimeType = "image/jpeg" };

      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Document = document1
      };

      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => _telegramBotClient.GetInfoAndDownloadFileAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).MustHaveHappened();
      A.CallTo(() => _teamWebService.UploadImage(A<UploadImageRequest>._)).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<Activity>._)).MustHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }


    [Fact]
    public async Task Begin_WithoutDocuments()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { GameId = 15, TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);

      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
      };

      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<string>._)).MustHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }
    [Fact]
    public async Task Begin_DocumentsIsNotImage()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { GameId = 15, TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      var document = new Document(){MimeType = "doc"};
      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Document = document
      };

      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<string>._)).MustHaveHappened();
      A.CallTo(() => _telegramBotClient.GetInfoAndDownloadFileAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }


    [Fact]
    public async Task Begin_NotInit()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      var document1 = new Document() { FileSize = 15, FileId = "15", MimeType = "image/jpeg" };

      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Document = document1
      };
      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<string>._)).MustHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }
  }
}