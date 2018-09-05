using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHunt.Model;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class RedeemDialogTest : BaseTest
    {
        private IActionWebService _actionWebService;
        private ILogger<RedeemDialog> _logger;
        private RedeemDialog _target;
        private IPasscodeWebService _passcodeWebService;
        private ITeamWebService _teamWebService;

        public RedeemDialogTest()
        {
            _testContainerBuilder.RegisterType<RedeemDialog>();
            _actionWebService = A.Fake<IActionWebService>();
            _passcodeWebService = A.Fake<IPasscodeWebService>();
            _teamWebService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_actionWebService).As<IActionWebService>();
            _testContainerBuilder.RegisterInstance(_passcodeWebService).As<IPasscodeWebService>();
            _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
            _logger = A.Fake<ILogger<RedeemDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<RedeemDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<RedeemDialog>();
        }

        [Theory]
        [InlineData(RedeemStatus.Ok)]
        [InlineData(RedeemStatus.AlreadyRedeem)]
        [InlineData(RedeemStatus.FullyRedeem)]
        [InlineData(RedeemStatus.WrongCode)]
        public async Task Begin(RedeemStatus redeemStatus)
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/redeem=YHTYTH"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var teamResponse = new TeamResponse()
            {
                Id = 16,
                GameId = 3
            };
            A.CallTo(() => _teamWebService.GetTeamForUserName(A<string>._)).Returns(teamResponse);
            A.CallTo(() => _passcodeWebService.RedeemPasscode(A<int>._, A<int>._, A<string>._))
                .Returns(new PasscodeResponse() {RedeemStatus = redeemStatus});
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _teamWebService.GetTeamForUserName(A<string>._)).MustHaveHappened();
            A.CallTo(() => _passcodeWebService.RedeemPasscode(teamResponse.GameId, teamResponse.Id, "YHTYTH"))
                .MustHaveHappened();
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
