using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class StartCommandTest : BaseTest<StartCommand>
    {
        private ILogger<IStartCommand> _logger;

        public StartCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IStartCommand>>());
            Build();
        }

        [Fact]
        public void Should_StartCommand_Execute_RedeemCommand()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }
    [Command("start")]
    public class StartCommand : AbstractCommand, IStartCommand
    {
        public StartCommand(ILogger<IStartCommand> logger) : base(logger)
        {
        }

        protected override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }
}
