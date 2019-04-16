using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class RefreshCommandTest : BaseTest<RefreshCommand>
    {
        private ILogger<IRefreshCommand> _logger;
        private INodeWebService _nodeWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IStringLocalizer<RefreshCommand> _localizer;

        public RefreshCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IRefreshCommand>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<RefreshCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_Refresh_Command_Refresh_Hidden_node_in_state()
        {
            // Arrange
            _state.HiddenNodes = new NodeResponse[]{new NodeResponse()};
            _state.Game = new GameResponse(){Id = 15};
            var hiddenNodes=new NodeResponse[]{new NodeResponse(), new NodeResponse(), new NodeResponse()};
            var actionNodes=new NodeResponse[]{new NodeResponse(), new NodeResponse(), new NodeResponse(), new NodeResponse()};
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Hidden, A<int>._)).Returns(hiddenNodes);
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Action, A<int>._)).Returns(actionNodes);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            Check.That(_state.HiddenNodes).Contains(hiddenNodes);
            Check.That(_state.ActionNodes).Contains(actionNodes);
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_RefreshCommand_Warn_if_Game_not_set()
        {
            // Arrange

            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();

        }
    }
}
