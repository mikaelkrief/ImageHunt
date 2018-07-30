using System.Collections.Generic;
using System.Text;
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
    public class BroadcastDialogTest : BaseTest
    {
        private ILogger<BroadcastDialog> _logger;
        private BroadcastDialog _target;

        public BroadcastDialogTest()
        {
            _testContainerBuilder.RegisterType<BroadcastDialog>();
            _logger = A.Fake<ILogger<BroadcastDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<BroadcastDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<BroadcastDialog>();

        }

        [Fact]
        public async Task Begin_BroadcastToGame()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/broadcast gameid=15 Toto!"
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
                Text = "/broadcast teamid=2 Toto!"
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
            A.CallTo(() => turnContext.SendActivity(A<IActivity>.That.Matches(a=>a.Text== "Toto!")))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => turnContext.End()).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
