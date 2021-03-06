﻿using System.Threading.Tasks;
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
    public class StartCommandTest : BaseTest<StartCommand>
    {
        private ILogger<IStartCommand> _logger;
        private ImageHuntState _state;
        private ITurnContext _turnContext;
        private IStringLocalizer<StartCommand> _localizer;

        public StartCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IStartCommand>>());
            TestContainerBuilder.RegisterInstance(A.Fake<IRedeemCommand>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<StartCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_StartCommand_Execute_RedeemCommand()
        {
            // Arrange
            var activity = new Activity(text: "/start redeem_gameId=21_pass=GFHFTF");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
        }
    }
}
