using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class ReceiveLocationDialogTest : BaseTest
    {
        private IReceiveLocationDialog _target;
        private ILogger _logger;
        private IActionWebService _actionWebService;

        public ReceiveLocationDialogTest()
        {
            _testContainerBuilder.RegisterType<ReceiveLocationDialog>();
            _actionWebService = A.Fake<IActionWebService>();
            _testContainerBuilder.RegisterInstance(_actionWebService).As<IActionWebService>();
            _logger = A.Fake<ILogger<ReceiveLocationDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<ReceiveLocationDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<ReceiveLocationDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Location = new Location() { Latitude = 15.6f, Longitude = 4.2f }
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._)).MustHaveHappened();
            Check.That(imageHuntState.CurrentLatitude).Equals(15.6f);
            Check.That(imageHuntState.CurrentLongitude).Equals(4.2f);
            A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._))
              .WithAnyArguments()
              .MustHaveHappened();
        }
        [Fact]
        public async Task Begin_GameNotStared()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Location = new Location() { Latitude = 15.6f, Longitude = 4.2f }
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState();
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._))
              .WithAnyArguments()
              .MustHaveHappened();
        }
        [Fact]
        public async Task Begin_CurrentLocation_In_range_current_Node()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Location = new Location() { Latitude = 15.6f, Longitude = 4.2f }
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started,
                CurrentNode = new NodeResponse()
                {
                    Latitude = 15.600005,
                    Longitude = 4.20004,
                    Name = "Node1",
                    ChildNodeIds = new List<int>() { 56 },
                    Points = 56
                }
            };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).MustHaveHappened();
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._)).MustHaveHappened();
            Check.That(imageHuntState.CurrentLatitude).Equals(15.6f);
            Check.That(imageHuntState.CurrentLongitude).Equals(4.2f);
            A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
                    A<Func<object, Exception, string>>._))
                .WithAnyArguments()
                .MustHaveHappened();
            A.CallTo(() => turnContext.ReplyActivity(A<string>._)).MustHaveHappened();
        }

    }
}
