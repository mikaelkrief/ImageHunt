using System;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
  public class ReceiveImageDialogTest : BaseTest
  {
    private ReceiveImageDialog _target;
    private ITeamWebService _teamWebService;
      
    public ReceiveImageDialogTest()
    {
      _testContainerBuilder.RegisterType<ReceiveImageDialog>();
      _teamWebService = A.Fake<ITeamWebService>();
      _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
      _container = _testContainerBuilder.Build();
      _target = _container.Resolve<ReceiveImageDialog>();
    }

    [Fact]
    public async Task Begin()
    {
      // Arrange
      var turnContext = A.Fake<ITurnContext>();
      var activity = new Activity(){ActivityType = ActivityType.Message, ChatId = 15, Picture = new Uri(@"Data\IMG_20170920_180905.jpg", UriKind.Relative)};

      A.CallTo(() => turnContext.Activity).Returns(activity);
      
      // Act
      await _target.Begin(turnContext);
      // Assert
    }
  }
}