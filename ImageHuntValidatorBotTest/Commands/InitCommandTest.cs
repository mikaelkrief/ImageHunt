using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotCore.Commands;
using ImageHuntValidator;
using ImageHuntValidatorBot.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntValidatorBotTest.Commands
{
    public class InitCommandTest : BaseTest<InitCommand>
    {
        private ILogger<IInitCommand> _logger;
        private IStringLocalizer<InitCommand> _localizer;
        private IGameWebService _gameWebService;
        private IActionWebService _actionWebService;
        private TurnContext _turnContext;

        public InitCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IInitCommand>>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<InitCommand>>());
            TestContainerBuilder.RegisterInstance(_gameWebService = A.Fake<IGameWebService>());
            _turnContext = A.Fake<TurnContext>();
        }

        [Fact]
        public void Should_InitCommand_Initialize_State()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }

    public class InitCommand : AbstractCommand<ImageHuntValidatorState>, IInitCommand
    {
        private readonly IGameWebService _gameWebService;

        public InitCommand(
            ILogger<IInitCommand> logger, 
            IStringLocalizer<InitCommand> localizer, 
            IGameWebService gameWebService) 
            : base(logger, localizer)
        {
            _gameWebService = gameWebService;
        }

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntValidatorState state)
        {
            var regex = new Regex(@"\/init\s?(gameid\s?=(?'gameid'\d*)|(teamid\s?=\s?(?'teamid'\d*)))");
        }
    }
}
