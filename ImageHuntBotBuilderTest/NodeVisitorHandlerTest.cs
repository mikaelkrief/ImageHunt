using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntCore.Computation;
using ImageHuntTelegramBot;
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

        public NodeVisitorHandlerTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<NodeVisitorHandler>>());
            _testContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            _testContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
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
                }
            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            var nextNode = await _target.MatchLocation(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();

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
                CurrentNode = new NodeResponse() { Latitude = 5.80003, Longitude = 5.869995}
            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            var nextNode = await _target.MatchLocation(_turnContext, state);
            // Assert
            Check.That(nextNode).IsNull();
        }
    }

    public class NodeVisitorHandler
    {
        private readonly ILogger<NodeVisitorHandler> _logger;
        private readonly INodeWebService _nodeWebService;
        private readonly ILifetimeScope _scope;
        private readonly IConfiguration _configuration;

        public NodeVisitorHandler(ILogger<NodeVisitorHandler> logger, INodeWebService nodeWebService, ILifetimeScope scope, IConfiguration configuration)
        {
            _logger = logger;
            _nodeWebService = nodeWebService;
            _scope = scope;
            _configuration = configuration;
        }

        public async Task<NodeResponse> MatchLocation(ITurnContext context, ImageHuntState state)
        {
            var activity = context.Activity;
            var location = activity.Attachments.First().Content as GeoCoordinates;
            // Check that location match the current node
            var distance = GeographyComputation.Distance(location.Latitude.Value, location.Longitude.Value, state.CurrentNode.Latitude,
                state.CurrentNode.Longitude);
            NodeResponse nextNode = null;
            var rangeDistance = Convert.ToDouble(_configuration["NodeSettings:RangeDistance"]);
            if (distance < rangeDistance)
            {
                await context.SendActivityAsync(
                    $"Vous avez rejoint le point de controle {state.CurrentNode.Name}, bravo!");
                switch (state.CurrentNode.NodeType)
                {
                    case NodeResponse.ObjectNodeType:
                        var nextNodeId = state.CurrentNode.ChildNodeIds.First();
                        nextNode = await _nodeWebService.GetNode(nextNodeId);
                        await context.SendActivityAsync($"Le prochain point de contrôle est {nextNode.Name}");
                        var nextActivity = new Activity(type: ImageHuntActivityTypes.Location,
                            attachments: new List<Attachment>()
                            {
                                new Attachment(ImageHuntActivityTypes.Location,
                                    content: new GeoCoordinates(latitude: nextNode.Latitude,
                                        longitude: nextNode.Longitude))
                            });
                        await context.SendActivityAsync(nextActivity);
                        break;
                }
            }
            return nextNode;
        }
    }
}
