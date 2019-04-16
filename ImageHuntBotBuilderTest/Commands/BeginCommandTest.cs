using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
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
    public class BeginCommandTest : BaseTest<BeginCommand>
    {
        private ITeamWebService _teamWebService;
        private IActionWebService _actionWebService;
        private ILogger<IBeginCommand> _logger;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IStringLocalizer<BeginCommand> _localizer;
        private INodeWebService _nodeWebService;

        public BeginCommandTest()
        {
            TestContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>()).AsImplementedInterfaces();
            TestContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>()).AsImplementedInterfaces();
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>()).AsImplementedInterfaces();
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IBeginCommand>>()).AsImplementedInterfaces();
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<BeginCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState(){GameId = 13, TeamId = 443, Game = new GameResponse()};
           Build();
        }

        [Fact]
        public async Task Should_Execute_BeginCommand_Start_Game()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Command, text: "/begin");
            _state.Status = Status.Initialized;

            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nodeResponse = new NodeResponse();
            A.CallTo(() => _teamWebService.StartGameForTeam(A<int>._, A<int>._, A<CancellationToken>._))
                .Returns(nodeResponse);
            _state.CurrentLocation = new GeoCoordinates();
            _state.Team = new TeamResponse(){CultureInfo = "fr"};
            var hiddenNodes = new NodeResponse[] { new NodeResponse(), new NodeResponse(), new NodeResponse() };
            var actionNodes = new NodeResponse[] { new NodeResponse(), new NodeResponse(), new NodeResponse(), new NodeResponse() };
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Hidden, A<int>._)).Returns(hiddenNodes);
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Action, A<int>._)).Returns(actionNodes);

            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _teamWebService.StartGameForTeam(A<int>._, A<int>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(_state.CurrentNode).Equals(nodeResponse);
            Check.That(_state.Status).Equals(Status.Started);
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Hidden, A<int>._)).MustHaveHappened();
            A.CallTo(() => _nodeWebService.GetNodesByType(NodeTypes.Action, A<int>._)).MustHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Execute_BeginCommand_Not_Start_Game_if_CurrentLocation_not_set()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Command, text: "/begin");
            _state.Status = Status.Initialized;
            _state.CurrentLocation = null;
            _state.Team = new TeamResponse(){CultureInfo = "fr"};

            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nodeResponse = new NodeResponse();
            A.CallTo(() => _teamWebService.StartGameForTeam(A<int>._, A<int>._, A<CancellationToken>._))
                .Returns(nodeResponse);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _teamWebService.StartGameForTeam(A<int>._, A<int>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            Check.That(_state.CurrentNode).IsNull();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_BeginCommand_Not_Start_Game_If_Status_Not_Initialized()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Command, text: "/begin");
            _state.Status = Status.None;
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _teamWebService.StartGameForTeam(A<int>._, A<int>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();

        }
    }
}
