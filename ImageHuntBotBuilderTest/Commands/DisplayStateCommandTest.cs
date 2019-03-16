using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
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
        private IStringLocalizer<DisplayStateCommand> _localizer;

        public DisplayStateCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDisplayStateCommand>>());
            _testContainerBuilder.RegisterInstance(_accessor = A.Fake<ImageHuntBotAccessors>());

            _statePropertyAccessor = A.Fake<IStatePropertyAccessor<ImageHuntState>>();

            _storage = A.Fake<IStorage>();
            _conversationState = new ConversationState(_storage);
            A.CallTo(() => _accessor.ImageHuntState).Returns(_statePropertyAccessor);
            _testContainerBuilder.RegisterInstance(_accessor);
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DisplayStateCommand>>());

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
                HiddenNodes = new[] {new NodeResponse() { Name = "Hidden1"}, new NodeResponse() { Name = "Hidden2"} },
                Status = Status.Started,
            };
            A.CallTo(() => _turnContext.Activity).Returns(new Activity(text: "/state"));
            // Act
            await _target.ExecuteAsync(_turnContext, state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_Display_All_State()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/state all");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>
            {
                new ImageHuntState() {GameId = 15, TeamId = 6, ConversationId = "Conv1"},
                new ImageHuntState() {GameId = 15, TeamId = 7, ConversationId = "Conv2"},
                new ImageHuntState() {GameId = 16, TeamId = 15, ConversationId = "Conv3"},
            };
            A.CallTo(() => _accessor.AllStates.GetAllAsync()).Returns(states);
            var state = new ImageHuntState();
            // Act
            await _target.ExecuteAsync(_turnContext, state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(states.Count, Times.Exactly);
        }
    }
}
