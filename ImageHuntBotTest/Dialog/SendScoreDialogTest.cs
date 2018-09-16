﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;
using ImageHuntWebServiceClient.Responses;


namespace ImageHuntBotTest.Dialog
{
    public class SendScoreDialogTest : BaseTest
    {
        private ILogger<SendScoreDialog> _logger;
        private SendScoreDialog _target;
        private IGameWebService _gameWebService;

        public SendScoreDialogTest()
        {
            _testContainerBuilder.RegisterType<SendScoreDialog>();
            _gameWebService = A.Fake<IGameWebService>();
            _testContainerBuilder.RegisterInstance(_gameWebService);
            _logger = A.Fake<ILogger<SendScoreDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<SendScoreDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<SendScoreDialog>();
        }

        [Fact]
        public async Task Begin_Send_to_Game()
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/sendScore gameid=15"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var states = new List<ImageHuntState> {
                new ImageHuntState(){GameId = 15, TeamId = 1},
                new ImageHuntState(){GameId = 15, TeamId = 2},
                new ImageHuntState(){GameId=16, TeamId = 1},
            };
            var scores = new List<ScoreResponse>()
            {
                new ScoreResponse(){Points = 130, Team = new TeamResponse(){GameId = 15, Id = 1, Name ="Team 1", Players = new PlayerResponse[]{new PlayerResponse(), new PlayerResponse() }}},
                new ScoreResponse(){Points = 140, Team = new TeamResponse(){GameId = 15, Id = 2, Name ="Team 2", Players = new PlayerResponse[]{new PlayerResponse(), new PlayerResponse() }}},
                new ScoreResponse(){Points = 150, Team = new TeamResponse(){GameId = 15, Id = 3, Name ="Team 3", Players = new PlayerResponse[]{new PlayerResponse(), new PlayerResponse() }}},
            };
            A.CallTo(() => turnContext.GetAllConversationState<ImageHuntState>()).Returns(states);
            A.CallTo(() => _gameWebService.GetScoresForGame(A<int>._, A<CancellationToken>._)).Returns(scores);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _gameWebService.GetScoresForGame(A<int>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => turnContext.SendActivity(A<Activity>._)).MustHaveHappened();
            A.CallTo(() => turnContext.End()).MustHaveHappened();
        }
    }
}