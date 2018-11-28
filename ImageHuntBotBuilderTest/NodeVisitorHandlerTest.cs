using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntTelegramBot;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using Activity = Microsoft.Bot.Schema.Activity;
using IActivity = Microsoft.Bot.Schema.IActivity;
using ITurnContext = Microsoft.Bot.Builder.ITurnContext;

namespace ImageHuntBotBuilderTest
{
    public class NodeVisitorHandlerTest : BaseTest<NodeVisitorHandler>
    {
        private ILogger<NodeVisitorHandler> _logger;
        private INodeWebService _nodeWebService;
        private IConfiguration _configuration;
        private ITurnContext _turnContext;
        private IActionWebService _actionWebService;

        public NodeVisitorHandlerTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<NodeVisitorHandler>>());
            _testContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            _testContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
            _testContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>());
            A.CallTo(() => _configuration["NodeSettings:RangeDistance"]).Returns("40");
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public async Task Should_location_match_node()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Location)
            {
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Content = new GeoCoordinates(latitude: 45.8, longitude: 5.87)
                    }
                }
            };
            var state = new ImageHuntState()
            {
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.8, Longitude = 5.87, NodeType = NodeResponse.ObjectNodeType,
                    ChildNodeIds = new List<int>() { 12},
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse();
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await _target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();
            Check.That(state.CurrentNode).IsEqualTo(nextNode).And.IsEqualTo(nextNodeExpected);
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_location_match_First_node()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Location)
            {
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Content = new GeoCoordinates(latitude: 45.8, longitude: 5.87)
                    }
                }
            };
            var state = new ImageHuntState()
            {
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999, Longitude = 5.86999,
                    ChildNodeIds = new List<int>() { 15},
                    NodeType = NodeResponse.FirstNodeType,
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse(){NodeType = "FirstNode"};
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await _target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();
            Check.That(state.CurrentNode).IsEqualTo(nextNode).And.IsEqualTo(nextNodeExpected);
        }
        [Fact]
        public async Task Should_location_match_Last_node()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Location)
            {
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Content = new GeoCoordinates(latitude: 45.8, longitude: 5.87)
                    }
                }
            };
            var state = new ImageHuntState()
            {
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999, Longitude = 5.86999,
                    NodeType = NodeResponse.LastNodeType,
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse(){NodeType = "FirstNode"};
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await _target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNull();
        }
        [Fact]
        public async Task Should_location_NOT_match_node()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.Location)
            {
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Content = new GeoCoordinates(latitude: 45.8, longitude: 5.87)
                    }
                }
            };
            var state = new ImageHuntState()
            {
                CurrentNode = new NodeResponse() { Latitude = 5.80003, Longitude = 5.869995},
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            var nextNode = await _target.MatchLocationAsync(_turnContext, state);
            // Assert
            Check.That(nextNode).IsNull();
        }
    }
}
