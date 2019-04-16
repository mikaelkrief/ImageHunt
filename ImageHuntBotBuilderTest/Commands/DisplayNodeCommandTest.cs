using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class DisplayNodeCommandTest : BaseTest<DisplayNodeCommand>
    {
        private ILogger<IDisplayNodeCommand> _logger;
        private INodeWebService _nodeWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IStringLocalizer<DisplayNodeCommand> _localizer;

        public DisplayNodeCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayNodeCommand>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DisplayNodeCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState(){Status = Status.Started};
            Build();
        }

        [Fact]
        public async Task Should_Display_node()
        {
            // Arrange
            _state.CurrentNode = new NodeResponse(){Id = 15};
            _state.CurrentNodeId = 15;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Send_error_message_if_CurrentNode_not_set()
        {
            // Arrange
            _state.CurrentNode = null;
            _state.CurrentNodeId = null;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Send_error_message_Game_not_started()
        {
            // Arrange
            _state.CurrentNode = new NodeResponse() { Id = 15 };
            _state.CurrentNodeId = 15;
            _state.Status = Status.None;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}
