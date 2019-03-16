using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class ResetCommandTest : BaseTest<ResetCommand>
    {
        private ILogger<IResetCommand> _logger;
        private ITurnContext _turnContext;
        private IStringLocalizer<ResetCommand> _localizer;

        public ResetCommandTest()
        {
            _logger = A.Fake<ILogger<IResetCommand>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ResetCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }
        [Fact]
        public async Task Should_Execute_Reset_State()
        {
            // Arrange
            var state = new ImageHuntState() {GameId = 15, TeamId = 65, Status = Status.Started};
            // Act
            await _target.ExecuteAsync(_turnContext, state);
            // Assert
            Check.That(state.GameId).IsNull();
        }
        [Fact]
        public async Task Should_Execute_Log_In_case_Exception()
        {
            // Arrange
            var state = new ImageHuntState() {GameId = 15, TeamId = 65, Status = Status.Started};
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .Throws<Exception>();
            // Act
            Check.ThatAsyncCode(() => _target.ExecuteAsync(_turnContext, state)).Throws<Exception>();
            // Assert
            A.CallTo(() => _logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                A < Func<object, Exception, string>>._)).MustHaveHappened();
        }

    }
}
