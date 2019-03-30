using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImagehuntBotBuilder;
using ImageHuntBotBuilder;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TestUtilities;
using Xunit;


namespace ImageHuntBotBuilderTest
{
    [Collection("AutomapperFixture")]
    public class TelegramAdapterTest : BaseTest
    {
        private TelegramAdapter _target;
        private IMapper _mapper;
        private ILogger<TelegramAdapter> _logger;
        private ITelegramBotClient _telegramBotClient;
        private IConfiguration _configuration;
        private ICredentialProvider _credentialProvider;
        private HttpClient _httpClient;
        private HttpMessageHandler _fakeHttpMessageHandler;
        private IImageWebService _imageWebService;

        public TelegramAdapterTest()
        {
            Startup.ConfigureMappings();
            _configuration = A.Fake<IConfiguration>();
            _credentialProvider = A.Fake<ICredentialProvider>();
            _testContainerBuilder.RegisterInstance(_credentialProvider);
            _testContainerBuilder.RegisterInstance(_configuration);
            _testContainerBuilder.RegisterType<TelegramAdapter>();
            _logger = A.Fake<ILogger<TelegramAdapter>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _mapper = Mapper.Instance;
            _testContainerBuilder.RegisterInstance(_mapper);
            _telegramBotClient = A.Fake<ITelegramBotClient>();
            _testContainerBuilder.RegisterInstance(_telegramBotClient);
            _fakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
            _httpClient = new HttpClient(_fakeHttpMessageHandler) { BaseAddress = new Uri("http://test.com") };
            _testContainerBuilder.RegisterInstance(_httpClient);
            _testContainerBuilder.RegisterInstance(_imageWebService = A.Fake<IImageWebService>());
            var container = _testContainerBuilder.Build();

            _target = container.Resolve<TelegramAdapter>();

            A.CallTo(() => _credentialProvider.IsAuthenticationDisabledAsync()).Returns(true);
        }

        [Fact]
        public void Should_Map_Update_to_Activity()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.sendText.json")
                    .ToObject<Update>();
            var fromExpected = new ChannelAccount(update.Message.From.Id.ToString(), update.Message.From.Username);
            // Act
            var activity = _mapper.Map<Activity>(update);
            // Assert
            Check.That(activity.ChannelId).Equals("telegram");
            Check.That(activity.Id).Equals(update.Message.Chat.Id.ToString());
            Check.That(activity.Type).Equals("message");
            Check.That(activity.From.Id).IsEqualTo(fromExpected.Id);
            Check.That(activity.From.Name).IsEqualTo(fromExpected.Name);
            Check.That(activity.Text).Equals(update.Message.Text);
            Check.That(activity.Value).Equals(update);
            Check.That(activity.Timestamp).Equals(new DateTimeOffset(update.Message.Date));
        }

        [Fact]
        public void Should_New_Participant()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.newUser.json")
                    .ToObject<Update>();
            var fromExpected = new ChannelAccount(update.Message.From.Id.ToString(), update.Message.From.Username);
            // Act
            var activity = _mapper.Map<Activity>(update);
            // Assert
            Check.That(activity.ChannelId).Equals("telegram");
            Check.That(activity.Id).Equals(update.Message.Chat.Id.ToString());
            Check.That(activity.Type).Equals(ImageHuntActivityTypes.NewPlayer);
            Check.That(activity.From.Id).IsEqualTo(fromExpected.Id);
            Check.That(activity.From.Name).IsEqualTo(fromExpected.Name);
            Check.That(activity.Text).Equals(update.Message.Text);
            Check.That(activity.Value).Equals(update);
            Check.That(activity.Timestamp).Equals(new DateTimeOffset(update.Message.Date));
        }

        [Fact]
        public void Should_Map_Update_to_Activity_Location()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(),
                        "ImageHuntBotBuilderTest.Data.sendLocation.json")
                    .ToObject<Update>();
            var mapper = Mapper.Instance;
            // Act
            var activity = _mapper.Map<Activity>(update);
            // Assert
            Check.That(activity.Id).Equals(update.EditedMessage.Chat.Id.ToString());
            Check.That(activity.Type).Equals("location");
            Check.That(activity.Attachments.Extracting("ContentType")).Contains("location");
            Check.That(activity.Attachments.Single().Content).IsInstanceOf<GeoCoordinates>();
            var location = activity.Attachments.Single().Content as GeoCoordinates;
            var expectedLocation = new GeoCoordinates(latitude: 47.875189d, longitude: 3.311155d);

            Check.That(location.Latitude).IsEqualsWithDelta(expectedLocation.Latitude, 0.001);
            Check.That(location.Longitude).IsEqualsWithDelta(expectedLocation.Longitude, 0.001);
        }

        [Fact]
        public void Should_Map_Update_to_Activity_image()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.sendImage.json")
                    .ToObject<Update>();
            var mapper = Mapper.Instance;
            // Act
            var activity = _mapper.Map<Activity>(update);
            // Assert
            Check.That(activity.Id).Equals(update.Message.Chat.Id.ToString());
            Check.That(activity.Type).Equals("message");
            Check.That(activity.Attachments.Extracting("ContentUrl"))
                .Contains("AgADBAADmK0xG3AuWVHxffLzbSk_BFNWoBoABIeSS3LyTp8lrXECAAEC");
        }

        [Fact]
        public async Task Should_Create_Activity_If_type_is_null()
        {
            // Arrange
            string authHeader = null;
            var callback = A.Fake<BotCallbackHandler>();
            A.CallTo(() => _configuration["BotConfiguration:BotUrl"]).Returns("http://localhost");
            Activity activity = new Activity()
            {
                Properties = GetJObjectFromResource(Assembly.GetExecutingAssembly(),
                    "ImageHuntBotBuilderTest.Data.sendImage.json"),
            };

            // Act
            await _target.ProcessActivityAsync(authHeader, activity, callback, CancellationToken.None);
            // Assert
            A.CallTo(() => callback.Invoke(A<ITurnContext>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_RunPipelineAsync_If_Activity_Type_NotNull()
        {
            // Arrange
            string authHeader = null;
            var callback = A.Fake<BotCallbackHandler>();

            Activity activity = new Activity() { Type = "message", ServiceUrl = "http://localhost", Text = "toto" };

            // Act
            await _target.ProcessActivityAsync(authHeader, activity, callback, CancellationToken.None);
            // Assert
            A.CallTo(() => callback.Invoke(A<ITurnContext>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_SendActivitesSync_Assert_If_bad_arguments()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            // Act
            Check.ThatAsyncCode(() => _target.SendActivitiesAsync(null, null, CancellationToken.None))
                .Throws<ArgumentNullException>();
            Check.ThatAsyncCode(() => _target.SendActivitiesAsync(turnContext, null, CancellationToken.None))
                .Throws<ArgumentNullException>();
            Check.ThatAsyncCode(() => _target.SendActivitiesAsync(turnContext, new Activity[0], CancellationToken.None))
                .Throws<ArgumentException>();
            // Assert
        }

        [Fact]
        public async Task Should_SendActivitiesAsync_on_emulator_return()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var connectorClient = A.Fake<IConnectorClient>();
            var conversation = A.Fake<IConversations>();
            A.CallTo(() => connectorClient.Conversations).Returns(conversation);
            var turnState = new TurnContextStateCollection() { { typeof(IConnectorClient).FullName, connectorClient } };
            A.CallTo(() => turnContext.TurnState).Returns(turnState);
            A.CallTo(() => conversation.SendToConversationWithHttpMessagesAsync(A<string>._, A<Activity>._,
                    A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(new HttpOperationResponse<ResourceResponse>());
            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = "message",
                    ChannelId = "emulator",
                    Id = "51515115",
                    Conversation = new ConversationAccount(),
                    Text = "toto"
                },
            };
            // Act
            var responses = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            Check.That(responses).HasSize(activities.Length);
            Check.That(responses.Extracting("Id")).Contains("51515115");
        }

        [Fact]
        public async Task Should_SendActivitiesAsync_on_telegram_return_Text()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var connectorClient = A.Fake<IConnectorClient>();
            var conversation = A.Fake<IConversations>();
            A.CallTo(() => connectorClient.Conversations).Returns(conversation);
            var turnState = new TurnContextStateCollection() { { typeof(IConnectorClient).FullName, connectorClient } };
            A.CallTo(() => turnContext.TurnState).Returns(turnState);
            A.CallTo(() => conversation.SendToConversationWithHttpMessagesAsync(A<string>._, A<Activity>._,
                    A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(new HttpOperationResponse<ResourceResponse>());
            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = "message",
                    ChannelId = "telegram",
                    Id = "151515",
                    Conversation = new ConversationAccount(),
                    Text = "toto"
                },
            };
            // Act
            var responses = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            Check.That(responses).HasSize(activities.Length);
            A.CallTo(() => _telegramBotClient.SendTextMessageAsync(A<ChatId>._, "toto", A<ParseMode>._, A<bool>._,
                A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_SendActivitiesAsync_on_telegram_Raise_exception()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var connectorClient = A.Fake<IConnectorClient>();
            var conversation = A.Fake<IConversations>();
            A.CallTo(() => connectorClient.Conversations).Returns(conversation);
            var turnState = new TurnContextStateCollection() { { typeof(IConnectorClient).FullName, connectorClient } };
            A.CallTo(() => turnContext.TurnState).Returns(turnState);
            A.CallTo(() => conversation.SendToConversationWithHttpMessagesAsync(A<string>._, A<Activity>._,
                    A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(new HttpOperationResponse<ResourceResponse>());
            A.CallTo(() => _telegramBotClient.SendTextMessageAsync(A<ChatId>._, A<string>._, A<ParseMode>._, A<bool>._,
                A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).ThrowsAsync(new ApiRequestException("Exception while sending message"));

            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = "message",
                    ChannelId = "telegram",
                    Id = "151515",
                    Conversation = new ConversationAccount(),
                    Text = "toto"
                },
            };
            // Act
            var responses = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            Check.That(responses).HasSize(activities.Length);
            A.CallTo(() => _telegramBotClient.SendTextMessageAsync(A<ChatId>._, "toto", A<ParseMode>._, A<bool>._,
                A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Download_Images_And_Store_in_activity_content_emulator()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    ContentType = "image/png",
                    ContentUrl =
                        "http://localhost:50354/v3/attachments/a3e895a0-e004-11e8-bd35-5d35ac6a130a/views/original",
                    Name = "Capture.png"
                }
            };
            var activity = new Activity()
            {
                Attachments = attachments,
                Type = ActivityTypes.Message,
                ServiceUrl = "http://localhost:50354"
            };
            var botCallback = A.Fake<BotCallbackHandler>();
            var response = new HttpResponseMessage();
            var content = new ByteArrayContent(new byte[15]);
            response.Content = content;
            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);
            // Act
            await _target.ProcessActivityAsync(null, activity, botCallback, CancellationToken.None);
            // Assert
            A.CallTo(() =>
                    botCallback.Invoke(A<ITurnContext>.That.Matches(t => CheckTurnContext(t)), A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Should_Download_Images_And_Store_in_activity_content_telegram()
        {
            // Arrange
            var attachments = new List<Attachment>
            {
                new Attachment()
                {
                    ContentType = "telegram/image",
                    ContentUrl =
                        "http://localhost:50354/v3/attachments/a3e895a0-e004-11e8-bd35-5d35ac6a130a/views/original",
                }
            };
            var activity = new Activity()
            {
                Attachments = attachments,
                Type = ActivityTypes.Message,
                ServiceUrl = "http://localhost:50354",
                Text = "Capture.jpg"
            };
            var botCallback = A.Fake<BotCallbackHandler>();
            var response = new HttpResponseMessage();
            var content = new ByteArrayContent(new byte[15]);
            response.Content = content;
            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);
            // Act
            await _target.ProcessActivityAsync(null, activity, botCallback, CancellationToken.None);
            // Assert
            A.CallTo(() =>
                    botCallback.Invoke(A<ITurnContext>.That.Matches(t => CheckTurnContext(t)), A<CancellationToken>._))
                .MustHaveHappened();
        }

        private bool CheckTurnContext(ITurnContext turnContext)
        {
            var activity = turnContext.Activity;
            Check.That(activity.Attachments.First().Content).IsNotNull();
            Check.That(activity.Type).Equals("image");
            return true;
        }

        [Fact]
        public async Task Should_Leave_ChatRoom()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();

            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = ImageHuntActivityTypes.Leave,
                    ChannelId = "telegram",
                    Id = "151515",
                    Conversation = new ConversationAccount(),
                    Text = "toto"
                },
            };
            // Act
            await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            A.CallTo(() => _telegramBotClient.LeaveChatAsync(A<ChatId>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Rename_Rename_Chat()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();

            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = ImageHuntActivityTypes.RenameChat,
                    ChannelId = "telegram",
                    Id = "151515",
                    Conversation = new ConversationAccount(),
                    Text = "toto"
                },

            };
            // Act
            await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            A.CallTo(
                    () => _telegramBotClient.SetChatTitleAsync(A<ChatId>._, activities[0].Text, A<CancellationToken>._))
                .MustHaveHappened();
        }
        [Fact]
        public async Task Should_Send_Location_To_Player()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();

            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = ImageHuntActivityTypes.Location,
                    ChannelId = "telegram",
                    Id = "151515",
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = ImageHuntActivityTypes.Location,
                            Content = new GeoCoordinates(latitude: 56.6, longitude: 5.34)
                        }
                    },
                    Conversation = new ConversationAccount(),
                },
            };
            // Act
            await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            A.CallTo(() => _telegramBotClient.SendLocationAsync(A<ChatId>._, A<float>._, A<float>._, A<int>._,
                A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Send_Image_To_Player()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = ImageHuntActivityTypes.Image,
                    ChannelId = "telegram",
                    Id = "151515",
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = ImageHuntActivityTypes.Image,
                            ContentUrl = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png"
                        }
                    },
                    Conversation = new ConversationAccount(),
                },
            };

            // Act
            await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            A.CallTo(() => _telegramBotClient.SendPhotoAsync(A<ChatId>._, A<InputOnlineFile>._, A<string>._,
                A<ParseMode>._, A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_wait_delayAsync()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activities = new Activity[]
            {
                new Activity()
                {
                    Type = ImageHuntActivityTypes.Wait,
                    ChannelId = "telegram",
                    Id = "151515",
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            Content = 10
                        }
                    },
                    Conversation = new ConversationAccount(),
                },
            };

            // Act
            await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert

        }

        [Fact]
        public async Task Should_Return_InviteUrlAsync()
        {
            // Arrange
            string authHeader = null;
            var turnContext = A.Fake<ITurnContext>();

            var inviteUrl = "https://tg.telegrame.com/invite";
            A.CallTo(() => _telegramBotClient.ExportChatInviteLinkAsync(A<ChatId>._, A<CancellationToken>._))
                .Returns(inviteUrl);
            var activities = new Activity[]
            {
                new Activity(type:ImageHuntActivityTypes.GetInviteLink)
                {
                    ChannelId = "telegram",
                    Id = "151515",
                    Conversation = new ConversationAccount(),

                }
            };

            // Act
            var result = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            A.CallTo(() => _telegramBotClient.ExportChatInviteLinkAsync(A<ChatId>._, A<CancellationToken>._))
                .MustHaveHappened();
            Check.That(activities[0].Attachments[0].ContentUrl).Equals(inviteUrl);
        }
    }
}