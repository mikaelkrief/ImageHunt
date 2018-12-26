using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Middlewares;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Middlewares
{
    public class LogPositionMiddlewareTest : BaseTest<LogPositionMiddleware>
    {
        private ILogger<LogPositionMiddleware> _logger;
        private ITurnContext _turnContext;
        private NextDelegate _nextDelegate;
        private IActionWebService _actionWebService;
        private ImageHuntBotAccessors _accessor;
        private IStatePropertyAccessor<ImageHuntState> _statePropertyAccessor;

        public LogPositionMiddlewareTest()
        {
            _logger = A.Fake<ILogger<LogPositionMiddleware>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _turnContext = A.Fake<ITurnContext>();
            _nextDelegate = A.Fake<NextDelegate>();
            _actionWebService = A.Fake<IActionWebService>();
            _testContainerBuilder.RegisterInstance(_actionWebService);
            _accessor = A.Fake<ImageHuntBotAccessors>();
            _testContainerBuilder.RegisterInstance(_accessor);
            _statePropertyAccessor = A.Fake<IStatePropertyAccessor<ImageHuntState>>();
            A.CallTo(() => _accessor.ImageHuntState).Returns(_statePropertyAccessor);
            Build();
        }

        [Fact]
        public async Task Should_Log_Position_And_Short_Circuit()
        {
            // Arrange
            var attachements = new List<Attachment>
            {
                new Attachment() {Content = new GeoCoordinates(latitude: 45.7, longitude: 65.9)}
            };
            var activity = new Activity() {Type = ImageHuntActivityTypes.Location, Attachments = attachements};
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started,
                GameId = 15,
                TeamId = 78,
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Log_Position_And_Transmit_to_Bot_if_position_close_to_current_node()
        {
            // Arrange
            var attachements = new List<Attachment>
            {
                new Attachment() {Content = new GeoCoordinates(latitude: 45.7, longitude: 65.9)}
            };
            var activity = new Activity() {Type = ImageHuntActivityTypes.Location, Attachments = attachements};
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started,
                GameId = 15,
                TeamId = 78,
                CurrentNode = new NodeResponse() { Latitude = 45.70001, Longitude = 65.89999}
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Do_Nothing_If_Activity_Type_not_location()
        {
            // Arrange
            var attachements = new List<Attachment>
            {
                new Attachment() {Content = new GeoCoordinates(latitude: 45.7, longitude: 65.9)}
            };
            var activity = new Activity() {Type = ActivityTypes.Message};
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started,
                GameId = 15,
                TeamId = 78,
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustHaveHappened();
        }
    }
}