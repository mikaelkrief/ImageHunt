using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class RedeemCommandTest : BaseTest<RedeemCommand>
    {
        private ILogger<IRedeemCommand> _logger;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IActionWebService _actionWebService;
        private IPasscodeWebService _passcodeWebService;
        private IStringLocalizer<RedeemCommand> _localizer;

        public RedeemCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IRedeemCommand>>());
            TestContainerBuilder.RegisterInstance(_passcodeWebService = A.Fake<IPasscodeWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<RedeemCommand>>());

            TestContainerBuilder.RegisterInstance(A.Fake<IBroadcastCommand>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_Execute_Redeem_Passcode_For_Team()
        {
            // Arrange
            var activity = new Activity(text: "/redeem gameid=45 pass=FGFGFFGT", from: new ChannelAccount(name:"toto"));
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _passcodeWebService.RedeemPasscode(45, "toto", "FGFGFFGT"));
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
