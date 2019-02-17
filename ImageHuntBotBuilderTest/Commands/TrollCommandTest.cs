﻿using System;
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
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class TrollCommandTest : BaseTest<TrollCommand>
    {
        private ILogger<ITrollCommand> _logger;
        private IStringLocalizer<TrollCommand> _localizer;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IActionWebService _actionWebService;

        public TrollCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>());
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<ITrollCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<TrollCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started, TeamId = 15, GameId = 5};

            Build();
        }

        [Fact]
        public async Task Should_Execute_Without_Error()
        {
            // Arrange
            
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_Not_Execute_if_team_not_set()
        {
            // Arrange
            _state.TeamId = null;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }
    }
}