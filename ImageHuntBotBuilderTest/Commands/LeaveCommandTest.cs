﻿using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
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

        public LeaveCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<ILeaveCommand>>());
            _turnContext = A.Fake<ITurnContext>();
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