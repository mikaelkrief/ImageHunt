using System;
using System.Collections.Generic;
using System.Text;
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
    public class StartCommandTest : BaseTest<StartCommand>
    {
        private ILogger<IStartCommand> _logger;
        private ImageHuntState _state;
        private ITurnContext _turnContext;

        public StartCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IStartCommand>>());
            _testContainerBuilder.RegisterInstance(A.Fake<IRedeemCommand>());
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public async Task Should_StartCommand_Execute_RedeemCommand()
        {
            // Arrange
            var activity = new Activity(text: "/start redeem_gameId=21_pass=GFHFTF");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
        }
    }
}
