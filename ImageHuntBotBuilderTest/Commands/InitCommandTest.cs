﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class InitCommandTest : BaseTest<InitCommand>
    {
        private ILogger<IInitCommand> _logger;
        private ITurnContext _turnContext;
        private ITeamWebService _teamWebService;
        private IGameWebService _gameWebService;

        public InitCommandTest()
        {
            _logger = A.Fake<ILogger<IInitCommand>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _gameWebService = A.Fake<IGameWebService>();
            _testContainerBuilder.RegisterInstance(_gameWebService);
            _teamWebService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_teamWebService);

            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public async Task Should_Execute_Set_GameId_And_TeamId_in_state()
        {
            // Arrange
            var activity = new Activity(text: "/init gameId=15 teamid=66");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._))
                .Returns(new GameResponse(){StartDate = DateTime.Now});
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).Returns(new TeamResponse(){CultureInfo = "fr-fr"});

            var state = new ImageHuntState();
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            Check.That(state.GameId).Equals(15);
            Check.That(state.TeamId).Equals(66);
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).MustHaveHappened();
            Check.That(state.Status).Equals(Status.Initialized);
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_Execute_unable_To_Get_Game_and_team_from_webservice()
        {
            // Arrange
            var activity = new Activity(text: "/init gameId=15 teamid=66");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).Returns<GameResponse>(null);
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).Returns<TeamResponse>(null);

            var state = new ImageHuntState();
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            Check.That(state.GameId).IsNull();
            Check.That(state.TeamId).IsNull();
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).MustHaveHappened();
            Check.That(state.Status).Equals(Status.None);
        }

        [Fact]
        public async Task Should_Execute_Warn_Group_Already_initialized()
        {
            // Arrange
            var activity = new Activity(text: "/init gameId=15 teamid=66");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var state = new ImageHuntState() {GameId = 15, TeamId = 6, Status = Status.Initialized};
            // Act
            await _target.Execute(_turnContext, state);
            // Assert
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).MustNotHaveHappened();
        }
    }
}