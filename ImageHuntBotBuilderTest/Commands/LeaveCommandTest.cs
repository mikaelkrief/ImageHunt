using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class LeaveCommandTest : BaseTest<LeaveCommand>
    {
        private ILogger<ILeaveCommand> _logger;
        private ImageHuntState _state;
        private ITurnContext _turnContext;
        private IStringLocalizer<LeaveCommand> _localizer;

        public LeaveCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<ILeaveCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<LeaveCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_Bot_Leave_Chat_On_Command()
        {
            // Arrange
            
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
