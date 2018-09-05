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
    public class StartDialogTest : BaseTest
    {
        private IPasscodeWebService _passcodeWebService;
        private ILogger<StartDialog> _logger;
        private StartDialog _target;
        private IRedeemDialog _redeemDialog;

        public StartDialogTest()
        {
            _testContainerBuilder.RegisterType<StartDialog>();
            _passcodeWebService = A.Fake<IPasscodeWebService>();
            _testContainerBuilder.RegisterInstance(_passcodeWebService);
            _logger = A.Fake<ILogger<StartDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<StartDialog>>();
            _redeemDialog = A.Fake<IRedeemDialog>();
            _testContainerBuilder.RegisterInstance(_redeemDialog).As<IRedeemDialog>();
            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<StartDialog>();

        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/start redeem=GFHFTF"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Initialized };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
    }
}
