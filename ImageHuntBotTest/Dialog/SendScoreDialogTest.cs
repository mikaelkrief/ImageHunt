using System;
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
    public class SendScoreDialogTest : BaseTest
    {
        private ILogger<SendScoreDialog> _logger;
        private SendScoreDialog _target;

        public SendScoreDialogTest()
        {
            _testContainerBuilder.RegisterType<SendScoreDialog>();
            _logger = A.Fake<ILogger<SendScoreDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<SendScoreDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<SendScoreDialog>();
        }

        [Fact]
        public async Task Begin_Send_to_Game()
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/sendScore gameid=15"
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
        }
    }

    public class SendScoreDialog : AbstractDialog, ISendScoreDialog
    {
        public SendScoreDialog(ILogger<SendScoreDialog> logger) : base(logger)
        {
        }

        public override string Command => "/sendScore";
    }
}
