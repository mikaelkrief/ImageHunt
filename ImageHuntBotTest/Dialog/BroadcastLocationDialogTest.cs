using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class BroadcastLocationDialogTest : BaseTest
    {
        private ILogger<BroadcastLocationDialog> _logger;
        private BroadcastLocationDialog _target;

        public BroadcastLocationDialogTest()
        {
            _testContainerBuilder.RegisterType<BroadcastLocationDialog>();
            _logger = A.Fake<ILogger<BroadcastLocationDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<BroadcastLocationDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<BroadcastLocationDialog>();

        }

        [Fact]
        public async Task Begin_BroadcastToGame()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/broadcastLocation gameid=15 Lat=45.56 Lng=3.6515"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState> {
                new ImageHuntState(){GameId = 15, TeamId = 1},
                new ImageHuntState(){GameId = 15, TeamId = 2},
                new ImageHuntState(){GameId=16, TeamId = 1},
            };
            A.CallTo(() => turnContext.GetAllConversationState<ImageHuntState>()).Returns(states);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.SendActivity(A<IActivity>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }
        [Fact]
        public async Task Begin_BroadcastToTeam()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/broadcastLocation teamid=2  Lat=45.56 Lng=3.6515"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState> {
                new ImageHuntState(){GameId = 15, TeamId = 1},
                new ImageHuntState(){GameId = 15, TeamId = 2},
                new ImageHuntState(){GameId=16, TeamId = 1},
            };
            A.CallTo(() => turnContext.GetAllConversationState<ImageHuntState>()).Returns(states);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.SendActivity(A<IActivity>._))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => turnContext.End()).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}