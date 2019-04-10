using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
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
        private IStringLocalizer<NodeVisitorHandler> _localizer;

        public NodeVisitorHandlerTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<NodeVisitorHandler>>());
            TestContainerBuilder.RegisterInstance(_nodeWebService = A.Fake<INodeWebService>());
            TestContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
            TestContainerBuilder.RegisterInstance(_actionWebService = A.Fake<IActionWebService>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<NodeVisitorHandler>>());
            A.CallTo(() => _configuration["NodeSettings:RangeDistance"]).Returns("40");
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public async Task Should_Location_match_Hidden_node()
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
                Status = Status.Started,
                Team = new TeamResponse(),
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.8,
                    Longitude = 5.87,
                    NodeType = NodeResponse.ObjectNodeType,
                    ChildNodeIds = new List<int>() { 12 },
                },
                HiddenNodes = new NodeResponse[]
                {
                    new NodeResponse(){Id = 2, NodeType = NodeResponse.BonusNodeType, Latitude = 45.8, Longitude = 5.87},
                    new NodeResponse(){Id = 3, NodeType = NodeResponse.HiddenNodeType, Latitude = 47.8, Longitude = 5.87},
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await Target.MatchHiddenNodesLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();

            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(state.HiddenNodes).HasSize(1);
        }
        [Fact]
        public async Task Should_not_crash_if_Hidden_node_null()
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
                    Latitude = 45.8,
                    Longitude = 5.87,
                    NodeType = NodeResponse.ObjectNodeType,
                    ChildNodeIds = new List<int>() { 12 },
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await Target.MatchHiddenNodesLocationAsync(_turnContext, state);
            // Assert
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
                Team = new TeamResponse(){CultureInfo = "fr"},

                Status = Status.Started,
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.8,
                    Longitude = 5.87,
                    NodeType = NodeResponse.ObjectNodeType,
                    ChildNodeIds = new List<int>() { 12 },
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse(){NodeType = "ObjectNode"};
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();
            Check.That(state.CurrentNode).IsEqualTo(nextNode).And.IsEqualTo(nextNodeExpected);
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_location_Do_nothing_if_game_not_started()
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
                Status = Status.Initialized,
                Team = new TeamResponse() { CultureInfo = "fr" },

                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.8,
                    Longitude = 5.87,
                    NodeType = NodeResponse.ObjectNodeType,
                    ChildNodeIds = new List<int>() { 12 },
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse(){NodeType = "ObjectNode"};
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustNotHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustNotHaveHappened();
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
                Status = Status.Started,
                Team = new TeamResponse() { CultureInfo = "fr" },

                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999,
                    Longitude = 5.86999,
                    ChildNodeIds = new List<int>() { 15 },
                    NodeType = NodeResponse.FirstNodeType,
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse() { NodeType = "ObjectNode" };
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();
            Check.That(state.CurrentNode).IsEqualTo(nextNode).And.IsEqualTo(nextNodeExpected);
        }
        [Fact]
        public async Task Should_location_match_Object_node()
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
                Status = Status.Started,
                Team = new TeamResponse() { CultureInfo = "fr" },

                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999,
                    Longitude = 5.86999,
                    ChildNodeIds = new List<int>() { 15 },
                    NodeType = NodeResponse.ObjectNodeType,
                    Action = "Toto"
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse() { NodeType = "ObjectNode" };
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(nextNode).IsNotNull();
            Check.That(state.CurrentNode).IsEqualTo(nextNode).And.IsEqualTo(nextNodeExpected);
        }
        [Fact]
        public async Task Should_Not_crash_if_CurrentNode_null()
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
                GameId = 45,
                TeamId = 87,
                Team = new TeamResponse() { CultureInfo = "fr" },

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse() { NodeType = "ObjectNode" };
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            Check.That(nextNode).IsNull();
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
                Status = Status.Started,
                Team = new TeamResponse() { CultureInfo = "fr" },

                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999,
                    Longitude = 5.86999,
                    NodeType = NodeResponse.LastNodeType,
                },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            var nextNodeExpected = new NodeResponse() { NodeType = "FirstNode" };
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).Returns(nextNodeExpected);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            Check.That(nextNode).IsNull();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public void Should_Send_Question()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                CurrentNode = new NodeResponse()
                {
                    Latitude = 45.79999,
                    Longitude = 5.86999,
                    NodeType = NodeResponse.ChoiceNodeType,
                    Question = "Question",
                    
                },
                GameId = 45,
                TeamId = 87,

            };
            // Act

            // Assert
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
                Team = new TeamResponse() { CultureInfo = "fr" },

                CurrentNode = new NodeResponse() { Latitude = 5.80003, Longitude = 5.869995 },
                GameId = 45,
                TeamId = 87,

            };
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            var nextNode = await Target.MatchLocationAsync(_turnContext, state);
            // Assert
            Check.That(nextNode).IsNull();
        }

        [Fact]
        public void Should_create_activity_from_ObjectNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.ObjectNodeType,
                Action = "Action to do",
                Latitude = 56.9,
                Longitude = 4.9,
                Name = "Action",
                Points = 56,
                Image = new ImageResponse() { PictureId = 15}
            };
            A.CallTo(() => _localizer[A<string>._]).Returns(new LocalizedString("Toto","{0}"));
            // Act
            var activities = Target.ActivitiesFromNode(node);
            // Assert
            A.CallTo(() => _localizer["NEXT_NODE_LOCATION", A<object[]>._]).MustHaveHappened();
            A.CallTo(() => _localizer["DO_ACTION_REQUEST", A<object[]>._]).MustHaveHappened();
            var expectedLocation = new GeoCoordinates(latitude: node.Latitude, longitude: node.Longitude);
            Check.That(((GeoCoordinates) activities.Single(a=>a.Type==ImageHuntActivityTypes.Location).Attachments
                .Single(a => a.ContentType == ImageHuntActivityTypes.Location).Content).Latitude).IsEqualTo(expectedLocation.Latitude);
            Check.That(((GeoCoordinates)activities.Single(a => a.Type == ImageHuntActivityTypes.Location).Attachments
                .Single(a => a.ContentType == ImageHuntActivityTypes.Location).Content).Longitude).IsEqualTo(expectedLocation.Longitude);
        }
        [Fact]
        public void Should_create_activity_from_WaypointNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.WaypointNodeType,
                Latitude = 56.9,
                Longitude = 4.9,
                Name = "Action",
            };
            A.CallTo(() => _localizer[A<string>._]).Returns(new LocalizedString("Toto", "{0}"));

            // Act
            var activities = Target.ActivitiesFromNode(node);
            // Assert
            A.CallTo(() => _localizer["NEXT_NODE_LOCATION", A<object[]>._]).MustHaveHappened();

            var expectedLocation = new GeoCoordinates(latitude: node.Latitude, longitude: node.Longitude);
            Check.That(((GeoCoordinates) activities.Single(a=>a.Type==ImageHuntActivityTypes.Location).Attachments
                .Single(a => a.ContentType == ImageHuntActivityTypes.Location).Content).Latitude).IsEqualTo(expectedLocation.Latitude);
            Check.That(((GeoCoordinates)activities.Single(a => a.Type == ImageHuntActivityTypes.Location).Attachments
                .Single(a => a.ContentType == ImageHuntActivityTypes.Location).Content).Longitude).IsEqualTo(expectedLocation.Longitude);
        }
        [Fact]
        public void Should_create_activity_from_HiddenNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.HiddenNodeType,
                Hint = "Hint to find",
                Latitude = 56.9,
                Longitude = 4.9,
                Name = "Hidden",
                Points = 56,
            };
            A.CallTo(() => _localizer[A<string>._]).Returns(new LocalizedString("Toto", "{0}"));

            // Act
            var activities = Target.ActivitiesFromNode(node);
            // Assert
            A.CallTo(() => _localizer["HIDDEN_NODE", A<object[]>._]).MustHaveHappened();
            Check.That(activities.Last().Text).Contains(node.Hint);
        }
        [Fact]
        public void Should_create_activity_from_LastNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.LastNodeType,
                Latitude = 56.9,
                Longitude = 4.9,
                Name = "Last",
                Points = 56,
            };
            // Act
            var activities = Target.ActivitiesFromNode(node);
            // Assert
            Check.That(activities).HasSize(2);

        }
        [Fact]
        public void Should_create_activity_from_TimerNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.TimerNodeType,
                Latitude = 56.9,
                Longitude = 4.9,
                Name = "Timer",
                Delay = 50,
            };
            // Act
            var activities = Target.ActivitiesFromNode(node);
            // Assert
            Check.That(activities.Single(a => a.Type == ImageHuntActivityTypes.Wait).Attachments.First().Content)
                .Equals(node.Delay);

        }
        [Fact]
        public async Task Should_create_DialogWaterfall_for_QuestionNode()
        {
            // Arrange
            var node = new NodeResponse()
            {
                NodeType = NodeResponse.QuestionNodeType,
                Name = "QuestionNode",
                Question = "The question",
                Latitude = 56.9,
                Longitude = 4.9,

                Delay = 50,
            };
            var state = new ImageHuntState()
            {
                CurrentNode = node,
                GameId = 45,
                TeamId = 87,

            };
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

            A.CallTo(() => _turnContext.Activity).Returns(activity);

            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();
            
            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, conversationState);
            // Assert

        }

        [Fact]
        public async Task Should_MatchLocationDialog_Return_null_if_current_node_null()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                GameId = 45,
                TeamId = 87,

            };
            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();

            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, conversationState);
            // Assert
            Check.That(state.CurrentDialog).IsNull();
        }
        [Fact]
        public async Task Should_MatchLocationDialog_Return_null_if_conversation_state_null()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                GameId = 45,
                TeamId = 87,
                CurrentNode = new NodeResponse() { NodeType = NodeResponse.BonusNodeType}
            };
            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();

            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, null);
            // Assert
            Check.That(state.CurrentDialog).IsNull();
        }
        [Fact]
        public async Task Should_MatchLocationDialog_Handle_Only_Question_And_Choice_Node()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                GameId = 45,
                TeamId = 87,
                CurrentNode = new NodeResponse() { NodeType = NodeResponse.BonusNodeType}
            };
            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();

            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, conversationState);
            // Assert
            Check.That(state.CurrentDialog).IsNull();
        }
        [Fact]
        public async Task Should_MatchLocationDialog_QuestionNode_Not_Reached()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                GameId = 45,
                TeamId = 87,
                CurrentNode = new NodeResponse() { NodeType = NodeResponse.QuestionNodeType, Latitude = 45.8, Longitude = 0}
            };
            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();
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

            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, conversationState);
            // Assert
            Check.That(state.CurrentDialog).IsNull();
        }
        [Fact]
        public async Task Should_MatchLocationDialog_QuestionNode_Reached()
        {
            // Arrange
            var state = new ImageHuntState()
            {
                GameId = 45,
                TeamId = 87,
                CurrentNode = new NodeResponse()
                {
                    NodeType = NodeResponse.QuestionNodeType,
                    Latitude = 45.8, Longitude = 5.87,
                    Question = "The Question",
                }
            };
            var conversationState = A.Fake<IStatePropertyAccessor<DialogState>>();
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

            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            await Target.MatchLocationDialogAsync(_turnContext, state, conversationState);
            // Assert
            Check.That(state.CurrentDialog).IsInstanceOf<DialogSet>();
        }
    }
}
