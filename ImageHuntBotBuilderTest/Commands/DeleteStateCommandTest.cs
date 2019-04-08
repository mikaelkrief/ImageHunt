using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class DeleteStateCommandTest : BaseTest<DeleteStateCommand>
    {
        private ILogger<IDeleteStateCommand> _logger;
        private IStringLocalizer<DeleteStateCommand> _localizer;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private ImageHuntBotAccessors _accessors;

        public DeleteStateCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDeleteStateCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DeleteStateCommand>>());
            _testContainerBuilder.RegisterInstance(_accessors = A.Fake<ImageHuntBotAccessors>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started };
            Build();
        }

        [Fact]
        public async Task Should_Delete_All_States_For_Game()
        {
            // Arrange
            Activity activity = new Activity(text:"/delState gameid=20");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>(){new ImageHuntState(){GameId = 20}, new ImageHuntState() { GameId = 20 }, new ImageHuntState() { GameId = 21 } };
            var statePropertyAccessor = A.Fake<IStatePropertyAccessorExtended<ImageHuntState>>();
            A.CallTo(() => statePropertyAccessor.GetAllAsync()).Returns(states);
            A.CallTo(() => _accessors.AllStates).Returns(statePropertyAccessor);
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _accessors.DeleteStateAsync(A<ITurnContext>._, A<CancellationToken>._))
                .MustHaveHappened(2, Times.Exactly);
        }
        [Fact]
        public async Task Should_Delete_All_States_For_Team()
        {
            // Arrange
            Activity activity = new Activity(text:"/delState teamid=20");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>(){new ImageHuntState(){TeamId = 20}, new ImageHuntState() { TeamId = 20 }, new ImageHuntState() { GameId = 21 } };
            var statePropertyAccessor = A.Fake<IStatePropertyAccessorExtended<ImageHuntState>>();
            A.CallTo(() => statePropertyAccessor.GetAllAsync()).Returns(states);
            A.CallTo(() => _accessors.AllStates).Returns(statePropertyAccessor);
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _accessors.DeleteStateAsync(A<ITurnContext>._, A<CancellationToken>._))
                .MustHaveHappened(2, Times.Exactly);
        }
    }
}
