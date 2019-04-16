using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;
using IActivity = Microsoft.Bot.Schema.IActivity;

namespace ImageHuntBotBuilderTest.Commands
{
    public class ActionCommandTest : BaseTest<ActionCommand>
    {
        private ILogger<IActionCommand> _logger;
        private INodeWebService _nodeWebService;
        private IStringLocalizer<ActionCommand> _localizer;
        private ITurnContext _turnContext;
        private ImageHuntState _state;

        public ActionCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IActionCommand>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ActionCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() {Status = Status.Started};
            Build();
        }

        [Fact]
        public async Task Should_Fail_if_Game_Not_Started()
        {
            // Arrange
            _state.Status = Status.Initialized;
            _state.GameId = 1;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _localizer["GAME_NOT_STARTED"]).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Display_Actions_Nodes()
        {
            // Arrange
            _state.GameId = 1;
            _state.ActionNodes = new NodeResponse[] {new NodeResponse(), new NodeResponse(),};
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _localizer["ACTION_NODES_TITLE"]).MustHaveHappened();
            A.CallTo(() => _turnContext.SendActivitiesAsync(A<IActivity[]>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}