using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class EndCommandTest : BaseTest<EndCommand>
    {
        private ILogger<IEndCommand> _logger;
        private IActionWebService _actionWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IStringLocalizer<EndCommand> _localizer;

        public EndCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IEndCommand>>());
            _testContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<EndCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_EndCommand_End_Game_And_Log_Action()
        {
            // Arrange
            var activity = new Activity(text: "/end");
            _state.GameId = 45;
            _state.TeamId = 778;
            _state.Status = Status.Started;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(_state.Status).Equals(Status.Ended);
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_EndCommand_Not_End_Game_If_Status_Not_Started()
        {
            // Arrange
            var activity = new Activity(text: "/end");
            _state.GameId = 45;
            _state.TeamId = 778;
            _state.Status = Status.Initialized;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            Check.That(_state.Status).Equals(Status.Initialized);
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();

        }
    }
}
