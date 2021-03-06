﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Responses;
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
    public class ResetNextNodeCommandTest : BaseTest<ResetNextNodeCommand>
    {
        private ILogger<IResetNextNodeCommand> _logger;
        private IStringLocalizer<ResetNextNodeCommand> _localizer;
        private INodeWebService _nodeWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IGameWebService _gameWebService;

        public ResetNextNodeCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IResetNextNodeCommand>>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ResetNextNodeCommand>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_gameWebService = A.Fake<IGameWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState()
            {
                Status = Status.Started, CurrentLocation = new GeoCoordinates(5,6),
                GameId = 5,
            };

            Build();
        }

        [Fact]
        public async Task Should_reply_Group_Not_Initialized()
        {
            // Arrange
            _state.Status = Status.None;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_reply_No_localization()
        {
            // Arrange
            _state.Status = Status.Started;
            _state.CurrentLocation = null;
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();

        }
        [Fact]
        public async Task Should_Replace_CurrentNode()
        {
            // Arrange
            var nodes = new List<NodeResponse>(){
                new NodeResponse(){Latitude = 4, Longitude = 6},
                new NodeResponse(){Latitude = 5, Longitude = 6},
                new NodeResponse(){Latitude = 6, Longitude = 6},
                new NodeResponse(){Latitude = 6, Longitude = 7},

            };
            _state.CurrentLocation = new GeoCoordinates(latitude:5, longitude:6);
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Path, A<int>._)).Returns(nodes);
            //A.CallTo(() => _gameWebService.GetPathNodesForGame(A<int>._, A<CancellationToken>._)).Returns(nodes);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Path, A<int>._)).MustHaveHappened();
            Check.That(_state.CurrentNode).Equals(nodes[1]);
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
