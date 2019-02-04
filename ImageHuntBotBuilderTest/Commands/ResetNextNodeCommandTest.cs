using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntCore.Computation;
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
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IResetNextNodeCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ResetNextNodeCommand>>());
            _testContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            _testContainerBuilder.RegisterInstance(_gameWebService = A.Fake<IGameWebService>());
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
            await _target.Execute(_turnContext, _state);
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
            await _target.Execute(_turnContext, _state);
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
            //A.CallTo(() => _gameWebService.GetPathNodesForGame(A<int>._, A<CancellationToken>._)).Returns(nodes);
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            //Check.That(_state.CurrentNode).Equals(nodes[1]);
            //A.CallTo(() => _gameWebService.GetPathNodesForGame(A<int>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();

        }
    }
    [Command("resetNext")]
    public class ResetNextNodeCommand : AbstractCommand, IResetNextNodeCommand
    {
        private readonly INodeWebService _nodeWebService;
        private readonly IGameWebService _gameWebService;

        public ResetNextNodeCommand(ILogger<IResetNextNodeCommand> logger, 
            IStringLocalizer<ResetNextNodeCommand> localizer, INodeWebService nodeWebService, 
            IGameWebService gameWebService) 
            : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
            _gameWebService = gameWebService;
        }

        public override bool IsAdmin => false;

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                _logger.LogError("Game not started");
                await turnContext.SendActivityAsync(string.Format(_localizer["GAME_NOT_STARTED"]));
                return;
            }

            if (state.CurrentLocation == null)
            {
                _logger.LogError("No team localisation");
                await turnContext.SendActivityAsync(string.Format(_localizer["NO_LOCALIZATION"]));
                return;
            }

            //var nodes = await _nodeWebService.;
        }
    }
}
