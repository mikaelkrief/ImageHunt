using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;


namespace ImageHuntBotBuilderTest.Commands
{
    public class GiveCommandTest : BaseTest<GiveCommand>
    {
        private ILogger<IGiveCommand> _logger;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IActionWebService _actionWebService;
        private IStringLocalizer<GiveCommand> _localizer;

        public GiveCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IGiveCommand>>());
            _testContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<GiveCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_Execute_Give_Points_To_Chatroom_s_Team()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/give points=15");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _state.GameId = 3;
            _state.TeamId = 15;
            _state.Status = Status.Started;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_Execute_Raise_Error_If_Team_or_game_not_set()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/give points=15");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _state.GameId = null;
            _state.TeamId = 15;
            _state.Status = Status.Started;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_Execute_Raise_Erro_If_Status_Not_Correct()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/give points=15");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _state.GameId = 65;
            _state.TeamId = 15;
            _state.Status = Status.None;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
