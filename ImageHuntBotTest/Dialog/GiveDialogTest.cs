using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class GiveDialogTest : BaseTest
    {
        private IActionWebService _actionWebService;
        private ILogger<GiveDialog> _logger;
        private GiveDialog _target;

        public GiveDialogTest()
        {
            _testContainerBuilder.RegisterType<GiveDialog>();
            _actionWebService = A.Fake<IActionWebService>();
            _testContainerBuilder.RegisterInstance(_actionWebService).As<IActionWebService>();
            _logger = A.Fake<ILogger<GiveDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<GiveDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<GiveDialog>();

        }

        [Fact]
        public async Task Give_Points()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/give points=150"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started, TeamId = 12};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Give_Negative_Points()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/give points=-150"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started, TeamId = 12};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
