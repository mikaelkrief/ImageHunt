using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class DisplayNodeDialogTest : BaseTest
    {
        private ILogger<DisplayNodeDialog> _logger;
        private DisplayNodeDialog _target;
        private INodeWebService _nodeWebService;

        public DisplayNodeDialogTest()
        {
            _testContainerBuilder.RegisterType<DisplayNodeDialog>();
            _logger = A.Fake<ILogger<DisplayNodeDialog>>();
            _nodeWebService = A.Fake<INodeWebService>();
            _testContainerBuilder.RegisterInstance(_nodeWebService);
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<DisplayNodeDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<DisplayNodeDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started, CurrentLatitude = 45.8, CurrentNodeId = 15 };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            var nodeResponse = new NodeResponse() { Name = "Toto", Latitude = 15.78, Longitude = 5.76, Action = "Titi" };
            A.CallTo(() => _nodeWebService.GetNode(imageHuntState.CurrentNodeId)).Returns(nodeResponse);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(imageHuntState.CurrentNodeId)).MustHaveHappened();
            A.CallTo(() => turnContext.SendActivity(A<IActivity>.That.Matches(a => CheckActivity(a, nodeResponse)))).MustHaveHappened();
            A.CallTo(() => turnContext.End()).MustHaveHappened();
        }

        private bool CheckActivity(IActivity activity, NodeResponse nodeResponse)
        {
            Check.That(activity.Location.Latitude).Equals((float)nodeResponse.Latitude);
            Check.That(activity.Location.Longitude).Equals((float)nodeResponse.Longitude);
            Check.That(activity.ActivityType).Equals(ActivityType.Message);
            return true;
        }
        [Fact]
        public async Task Begin_Game_Not_Started()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Initialized, CurrentLatitude = 45.8, CurrentNodeId = 15 };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            var nodeResponse = new NodeResponse() { Name = "Toto", Latitude = 15.78, Longitude = 5.76, Action = "Titi" };
            A.CallTo(() => _nodeWebService.GetNode(imageHuntState.CurrentNodeId)).Returns(nodeResponse);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.ReplyActivity(A<IActivity>._)).MustHaveHappened();
            A.CallTo(() => turnContext.End()).MustHaveHappened();
        }

        [Fact]
        public async Task Begin_No_Current_Node()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _nodeWebService.GetNode(A<int>._)).MustNotHaveHappened();
            A.CallTo(() => turnContext.End()).MustHaveHappened();
        }
    }
}
