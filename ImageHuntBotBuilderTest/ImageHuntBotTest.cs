using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using FakeItEasy;
using ImageHunt;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore;
using ImageHuntBotCore.Commands;
using ImageHuntBotCore.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    [Collection("AutomapperFixture")]
    public class ImageHuntBotTest : BaseTest<ImageHuntBotBuilder.ImageHuntBot>
    {
        private ILogger<ImageHuntBotBuilder.ImageHuntBot> _logger;
        private ImageHuntBotAccessors _accessor;
        private ITurnContext _turnContext;
        private IStatePropertyAccessor<ImageHuntState> _statePropertyAccessor;
        private IActionWebService _actionWebService;
        private IStorage _storage;
        private ConversationState _conversationState;
        private ITeamWebService _teamWebService;
        private ICommandRepository<ImageHuntState> _commandRepository;
        private Activity _activity;
        private ImageHuntState _state;
        private INodeVisitorHandler _nodevisitorHandler;
        private IStringLocalizer<ImageHuntBot> _localizer;

        public ImageHuntBotTest()
        {
            Startup.ConfigureMappings();
            _logger = A.Fake<ILogger<ImageHuntBotBuilder.ImageHuntBot>>();
            TestContainerBuilder.RegisterInstance(_logger);
            _turnContext = A.Fake<ITurnContext>();
            _actionWebService = A.Fake<IActionWebService>();
            _teamWebService = A.Fake<ITeamWebService>();
            TestContainerBuilder.RegisterInstance(_actionWebService);
            TestContainerBuilder.RegisterInstance(_teamWebService);
            TestContainerBuilder.RegisterInstance(_nodevisitorHandler = A.Fake<INodeVisitorHandler>());
            _statePropertyAccessor = A.Fake<IStatePropertyAccessor<ImageHuntState>>();
            _storage = A.Fake<IStorage>();
            _conversationState = new ConversationState(_storage);
            _accessor = new ImageHuntBotAccessors(_conversationState);
            _accessor.ImageHuntState = _statePropertyAccessor;
            _accessor.ConversationDialogState = A.Fake<IStatePropertyAccessor<DialogState>>();
            TestContainerBuilder.RegisterInstance(_accessor);
            _commandRepository = A.Fake<ICommandRepository<ImageHuntState>>();
            TestContainerBuilder.RegisterInstance(_commandRepository);
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ImageHuntBot>>());
            _activity = new Activity() {Conversation = new ConversationAccount(){Id = "toto|livechat"}};
            _state = new ImageHuntState();
            Build();
        }


        [Fact]
        public async Task Should_Not_Turn_Record_Position_If_Game_Not_Started()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    ContentType = "location",
                    Content = new GeoCoordinates(latitude: 15.56d, longitude: 3.92)
                }
            };
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.None
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);
            _activity.Type = ImageHuntActivityTypes.Location;
            _activity.Attachments = attachments;
            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Should_Not_Turn_Record_Position_If_GameId_And_TeamId_not_set()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    ContentType = ImageHuntActivityTypes.Location,
                    Content = new GeoCoordinates(latitude: 15.56d, longitude: 3.92)
                }
            };
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);
            _activity.Type = ImageHuntActivityTypes.Location;
            _activity.Attachments = attachments;
            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _actionWebService.LogPosition(A<LogPositionRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Should_Turn_Record_Images()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    Content = 15
                }
            };
            _activity.Type = ImageHuntActivityTypes.Image;
            _activity.Attachments = attachments;
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.Started,
                GameId = 15,
                TeamId = 3,
                CurrentLocation = new GeoCoordinates(latitude: 15.6d, longitude: 3.9d)
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);

            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _actionWebService.LogAction(A<GameActionRequest>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Turn_Not_Record_Images_If_Hunt_Not_Started()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    Content = new byte[15]
                }
            };
            _activity.Type = ImageHuntActivityTypes.Image;
            _activity.Attachments = attachments;
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.None,
                GameId = 15,
                TeamId = 3,
                CurrentLocation = new GeoCoordinates(latitude: 15.6d, longitude: 3.9d)
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);

            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _teamWebService.UploadImage(A<UploadImageRequest>._)).MustNotHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Bot_Handle_Command()
        {
            // Arrange
            _activity.Type = ActivityTypes.Message;
            _activity.Text ="/toto";
            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            var command = A.Fake<ICommand<ImageHuntState>>();
            A.CallTo(() => _commandRepository.Get(_turnContext, A<ImageHuntState>._, _activity.Text)).Returns(command);

            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _commandRepository.Get(_turnContext, A<ImageHuntState>._, _activity.Text)).MustHaveHappened();
            A.CallTo(() => command.ExecuteAsync(_turnContext, A<ImageHuntState>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Bot_Not_execute_command_if_user_not_authorized()
        {
            // Arrange
            _activity.Type = ActivityTypes.Message;
            _activity.Text ="/toto";
            _activity.From = new ChannelAccount(name:"User");
            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            var command = A.Fake<ICommand>();
            A.CallTo(() => _commandRepository.Get(A<ITurnContext>._, A<ImageHuntState>._, A<string>._)).Throws(new NotAuthorizedException("User not authorized"));

            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _commandRepository.Get(_turnContext, A<ImageHuntState>._, _activity.Text)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_Bot_handle_Bad_command()
        {
            // Arrange
            _activity.Type = ActivityTypes.Message;
            _activity.Text ="/toto";
            _activity.From = new ChannelAccount(name:"User");
            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            var command = A.Fake<ICommand>();
            A.CallTo(() => _commandRepository.Get(A<ITurnContext>._, A<ImageHuntState>._, A<string>._)).Throws(new CommandNotFound("toto"));

            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(() => _commandRepository.Get(_turnContext, A<ImageHuntState>._, _activity.Text)).MustHaveHappened();
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_Turn_Handle_position_if_close_current_node()
        {
            // Arrange
            var location = new GeoCoordinates(latitude: 15.6d, longitude: 3.9d);
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    Content = location
                }
            };
            _activity.Type = ImageHuntActivityTypes.Location;
            _activity.Attachments = attachments;
            var imageHuntState = new ImageHuntState()
            {
                Status = Status.None,
                GameId = 15,
                TeamId = 3,
                CurrentLocation = location
            };
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(imageHuntState);

            A.CallTo(() => _turnContext.Activity).Returns(_activity);
            // Act
            await Target.OnTurnAsync(_turnContext);
            // Assert
            A.CallTo(
                    () => _nodevisitorHandler.MatchLocationAsync(A<ITurnContext>._, A<ImageHuntState>._))
                .MustHaveHappened();
            A.CallTo(() => _nodevisitorHandler.MatchHiddenNodesLocationAsync(A<ITurnContext>._, A<ImageHuntState>._))
                .MustHaveHappened();
        }

    }
}