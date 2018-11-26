using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class DisplayStateCommandTest : BaseTest<DisplayStateCommand>
    {
        private ILogger<IDisplayStateCommand> _logger;
        private IStatePropertyAccessor<ImageHuntState> _statePropertyAccessor;
        private IStorage _storage;
        private ConversationState _conversationState;
        private ImageHuntBotAccessors _accessor;
        private ITurnContext _turnContext;

        public DisplayStateCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayStateCommand>>());
            _statePropertyAccessor = A.Fake<IStatePropertyAccessor<ImageHuntState>>();

            _storage = A.Fake<IStorage>();
            _conversationState = new ConversationState(_storage);
            _accessor = new ImageHuntBotAccessors(_conversationState);
            _accessor.ImageHuntState = _statePropertyAccessor;
            _testContainerBuilder.RegisterInstance(_accessor);

            _turnContext = A.Fake<ITurnContext>();

            Build();
        }

        [Fact]
        public async Task Should_Display_State()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                Game = new GameResponse() { Id = 65, IsActive = true, Name = "GameName", PictureId = 45, StartDate = DateTime.Now},
                Team = new TeamResponse() { GameId = 65, Id = 45, Name = "TeamName", Players = new PlayerResponse[]{new PlayerResponse(){Id = 3, Name = "Player1"},new PlayerResponse(){Id = 3, Name = "Player2"}, }},
                CurrentLocation = new GeoCoordinates(latitude:45.7d, longitude:6.87d),
                CurrentNode = new NodeResponse() { Name = "NodeName", Id = 56, ChildNodeIds = new int[]{45, 67}},
                Status = Status.Started,
            };
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
