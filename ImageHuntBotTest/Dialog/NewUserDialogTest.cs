using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class NewUserDialogTest : BaseTest
    {
        private ILogger<NewUserDialog> _logger;
        private ITeamWebService _teamWebService;
        private NewUserDialog _target;

        public NewUserDialogTest()
        {
            _testContainerBuilder.RegisterType<NewUserDialog>();
            _logger = A.Fake<ILogger<NewUserDialog>>();
            _teamWebService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_teamWebService);
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<NewUserDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<NewUserDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var state = new ImageHuntState(){Status = Status.Initialized, ChatId = 15, GameId = 2, TeamId = 4};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            var activity = new Activity(){};
            A.CallTo(() => turnContext.Activity).Returns(activity);
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
        [Fact]
        public async Task Begin_Non_Initialized_Group()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var state = new ImageHuntState();
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            var activity = new Activity(){};
            A.CallTo(() => turnContext.Activity).Returns(activity);
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
    }
}
