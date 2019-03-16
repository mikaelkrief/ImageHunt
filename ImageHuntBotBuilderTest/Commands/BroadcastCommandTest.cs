using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;
using Activity = Microsoft.Bot.Schema.Activity;
using ITurnContext = Microsoft.Bot.Builder.ITurnContext;

namespace ImageHuntBotBuilderTest.Commands
{
    public class BroadcastCommandTest : BaseTest<BroadcastCommand>
    {
        private ILogger<IBroadcastCommand> _logger;
        private ITurnContext _turnContext;
        private ImageHuntBotAccessors _accessors;
        private IStringLocalizer<BroadcastCommand> _localizer;

        public BroadcastCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IBroadcastCommand>>());
            _testContainerBuilder.RegisterInstance(_accessors = A.Fake<ImageHuntBotAccessors>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<BroadcastCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public async Task Should_Broadcast_Do_Nothing_if_command_misspeled()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/broadcast gmeid=15 Toto");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>
            {
                new ImageHuntState() {GameId = 15, TeamId = 6, ConversationId = "Conv1"},
                new ImageHuntState() {GameId = 15, TeamId = 7, ConversationId = "Conv2"},
                new ImageHuntState() {GameId = 16, TeamId = 15, ConversationId = "Conv3"},
            };
            A.CallTo(() => _accessors.AllStates.GetAllAsync()).Returns(states);
            var state = new ImageHuntState();
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            A.CallTo(() =>
                _turnContext.SendActivityAsync(
                    A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Broadcast_Dispatch_Text_On_All_Conversations()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/broadcast gameid=15 Toto");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>
            {
                new ImageHuntState() {GameId = 15, TeamId = 6, ConversationId = "Conv1"},
                new ImageHuntState() {GameId = 15, TeamId = 7, ConversationId = "Conv2"},
                new ImageHuntState() {GameId = 16, TeamId = 15, ConversationId = "Conv3"},
            };
            A.CallTo(() => _accessors.AllStates.GetAllAsync()).Returns(states);
            var state = new ImageHuntState();
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            A.CallTo(() =>
                _turnContext.SendActivitiesAsync(
                    A<IActivity[]>.That.Matches(a => a.First().Conversation.Id == states[0].ConversationId),
                    A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() =>
                _turnContext.SendActivitiesAsync(
                    A<IActivity[]>.That.Matches(a=>a.Length == 2),
                    A<CancellationToken>._)).MustHaveHappened(Repeated.Exactly.Once);

        }
        [Fact]
        public async Task Should_Broadcast_Dispatch_Text_For_Specific_team()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/broadcast teamid=15 Toto");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState>
            {
                new ImageHuntState() {GameId = 15, TeamId = 6, ConversationId = "Conv1"},
                new ImageHuntState() {GameId = 15, TeamId = 7, ConversationId = "Conv2"},
                new ImageHuntState() {GameId = 16, TeamId = 15, ConversationId = "Conv3"},
            };
            A.CallTo(() => _accessors.AllStates.GetAllAsync()).Returns(states);
            var state = new ImageHuntState();
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            A.CallTo(() =>
                _turnContext.SendActivitiesAsync(
                    A<IActivity[]>.That.Matches(a => a.First().Conversation.Id == states[2].ConversationId),
                    A<CancellationToken>._)).MustHaveHappened();
        }
    }
}
