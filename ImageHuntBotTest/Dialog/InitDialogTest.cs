using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntTelegramBot.Dialogs.Prompts;
using ImageHuntWebServiceClient.WebServices;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class InitDialogTest : BaseTest
    {
      private IInitDialog _target;
      private IGameWebService _gameWebService;
      private ITeamWebService _teamWebService;

      public InitDialogTest()
      {
        _testContainerBuilder.RegisterType<InitDialog>().As<IInitDialog>();
        _gameWebService = A.Fake<IGameWebService>();
        _testContainerBuilder.RegisterInstance(_gameWebService);
        _teamWebService = A.Fake<ITeamWebService>();
        _testContainerBuilder.RegisterInstance(_teamWebService);
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<IInitDialog>();
      }
      [Fact]
      public async Task Begin_Call_Sub_Dialogs()
      {
        // Arrange
        var context = A.Fake<ITurnContext>();
        var state = new ImageHuntState();
        var activity = new Activity(){Text = "/init gameid=2 teamid=6"};
        A.CallTo(() => context.GetConversationState<ImageHuntState>()).Returns(state);
        A.CallTo(() => context.Activity).Returns(activity);
        // Act
        await _target.Begin(context);
        // Assert
        //A.CallTo(() => context.Begin(A<NumberPrompt<int>>._)).MustHaveHappened();
        Check.That(state.GameId).Equals(2);
        Check.That(state.TeamId).Equals(6);
        A.CallTo(() => _gameWebService.GetGameById(2, A<CancellationToken>._)).MustHaveHappened();
        A.CallTo(() => _teamWebService.GetTeamById(6)).MustHaveHappened();
      }

  }
}
