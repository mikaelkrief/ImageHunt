using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class DisplayHintsCommandTest : BaseTest<DisplayHintsCommand>
    {
        private ILogger<IDisplayHintsCommand> _logger;
        private INodeWebService _nodeWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;

        public DisplayHintsCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayHintsCommand>>());
            _testContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started };
            Build();
        }

        [Fact]
        public async Task Should_Display_Hints_For_HiddenNodes()
        {
            // Arrange
            _state.GameId = 1;

            _state.HiddenNodes = new []
            {
                new NodeResponse(){NodeType = NodeResponse.BonusNodeType, Name = "bonus1", BonusType = BonusNode.BONUS_TYPE.Points_x2, Hint = "Hint1"},
                new NodeResponse(){NodeType = NodeResponse.HiddenNodeType, Name = "Hidden1", Points = 15, Hint = "Hint2"},
            };
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Times(_state.HiddenNodes.Length + 1));
        }
        [Fact]
        public async Task Should_Display_Hints_Reply_nothing_if_no_hidden_node_remains()
        {
            // Arrange
            _state.GameId = 1;

            _state.HiddenNodes = new NodeResponse[0];
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Times(_state.HiddenNodes.Length + 1));
        }
        [Fact]
        public async Task Should_Warn_User_if_Game_Not_Started()
        {
            // Arrange
            _state.Status = Status.Initialized;
            _state.GameId = 1;
            _state.HiddenNodes = new []
            {
                new NodeResponse(){NodeType = NodeResponse.BonusNodeType, Name = "bonus1", BonusType = BonusNode.BONUS_TYPE.Points_x2, Hint = "Hint1"},
                new NodeResponse(){NodeType = NodeResponse.HiddenNodeType, Name = "Hidden1", Points = 15, Hint = "Hint2"},
            };
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNodesByType(A<NodeTypes>._, A<int>._)).MustNotHaveHappened();
        }
    }
}
