using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
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
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayNodeCommand>>());
            _testContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DisplayNodeCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState(){Status = Status.Started};
            Build();
        }

        [Fact]
        public async Task Should_Display_node()
        {
            // Arrange
            _state.CurrentNodeId = 15;
            // Act
            await _target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Send_error_message_if_CurrentNode_not_set()
        {
            // Arrange
            _state.CurrentNodeId = null;
            // Act
            await _target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Send_error_message_Game_not_started()
        {
            // Arrange
            _state.CurrentNodeId = 15;
            _state.Status = Status.None;
            // Act
            await _target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(15)).MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}
