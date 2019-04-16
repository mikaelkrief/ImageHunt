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
        private IStringLocalizer<DisplayHintsCommand> _localizer;

        public DisplayHintsCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayHintsCommand>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DisplayHintsCommand>>());

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
                new NodeResponse(){NodeType = NodeResponse.BonusNodeType, Name = "bonus1", BonusType = BonusNode.BONUSTYPE.PointsX2, Hint = "Hint1"},
                new NodeResponse(){NodeType = NodeResponse.HiddenNodeType, Name = "Hidden1", Points = 15, Hint = "Hint2"},
            };
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
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
            await Target.ExecuteAsync(_turnContext, _state);
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
                new NodeResponse(){NodeType = NodeResponse.BonusNodeType, Name = "bonus1", BonusType = BonusNode.BONUSTYPE.PointsX2, Hint = "Hint1"},
                new NodeResponse(){NodeType = NodeResponse.HiddenNodeType, Name = "Hidden1", Points = 15, Hint = "Hint2"},
            };
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNodesByType(A<NodeTypes>._, A<int>._)).MustNotHaveHappened();
        }
    }
}
