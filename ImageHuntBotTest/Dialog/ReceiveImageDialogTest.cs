using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;
using File = Telegram.Bot.Types.File;

namespace ImageHuntBotTest
{
  public class ReceiveImageDialogTest : BaseTest
  {
    private ReceiveImageDialog _target;
    private ITeamWebService _teamWebService;
    private ITelegramBotClient _telegramBotClient;

    public ReceiveImageDialogTest()
    {
      _testContainerBuilder.RegisterType<ReceiveImageDialog>();
      _teamWebService = A.Fake<ITeamWebService>();
      _telegramBotClient = A.Fake<ITelegramBotClient>();
      _testContainerBuilder.RegisterInstance(_telegramBotClient).As<ITelegramBotClient>();

      _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
      _container = _testContainerBuilder.Build();
      _target = _container.Resolve<ReceiveImageDialog>();
    }

    [Fact]
    public async Task Begin()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { GameId = 15, TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      var photoSize1 = new PhotoSize(){FileSize = 15, FileId = "15"};
      var photoSize2 = new PhotoSize(){FileSize = 1195247, FileId = "AgADBAADOawxG-RQqVO-4ni8OVZOPOnykBkABDQFk1xY-YUAAR0SAgABAg" };
      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Pictures = new[]
        {
          photoSize1,
          photoSize2
        }
      };

      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => _telegramBotClient.GetInfoAndDownloadFileAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).MustHaveHappened();
      A.CallTo(() => _teamWebService.UploadImage(A<ImageHuntWebServiceClient.Request.UploadImageRequest>._)).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<Activity>._)).MustHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }
    [Fact]
    public async Task Begin_With_ImageName()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { GameId = 15, TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      var photoSize1 = new PhotoSize(){FileSize = 15, FileId = "15"};
      var photoSize2 = new PhotoSize(){FileSize = 1195247, FileId = "AgADBAADOawxG-RQqVO-4ni8OVZOPOnykBkABDQFk1xY-YUAAR0SAgABAg" };
      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Pictures = new[]
        {
          photoSize1,
          photoSize2
        },
        Text = "3"
      };

      A.CallTo(() => turnContext.Activity).Returns(activity);

      // Act
      await _target.Begin(turnContext);
      // Assert
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
      A.CallTo(() => _telegramBotClient.GetInfoAndDownloadFileAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).MustHaveHappened();
      A.CallTo(() => _teamWebService.UploadImage(A<UploadImageRequest>.That.Matches(r=>CheckImageHuntRequest(r, activity.Text)))).MustHaveHappened();
      A.CallTo(() => turnContext.ReplyActivity(A<Activity>._)).MustHaveHappened();
      A.CallTo(() => turnContext.End()).MustHaveHappened();
    }

    private bool CheckImageHuntRequest(UploadImageRequest uploadImageRequest, string imageName)
    {
      Check.That(uploadImageRequest.ImageName).Equals(imageName);
      return true;
    }

    [Fact]
    public async Task Begin_NotInit()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var imageHuntState = new ImageHuntState() { TeamId = 16, CurrentLatitude = 15.2, CurrentLongitude = 56};
      A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
      var photoSize1 = new PhotoSize(){FileSize = 15, FileId = "15"};
      var photoSize2 = new PhotoSize(){FileSize = 1195247, FileId = "AgADBAADOawxG-RQqVO-4ni8OVZOPOnykBkABDQFk1xY-YUAAR0SAgABAg" };
      var activity = new Activity()
      {
        ActivityType = ActivityType.Message,
        ChatId = 15,
        Pictures = new[]
        {
          photoSize1,
          photoSize2
        }
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