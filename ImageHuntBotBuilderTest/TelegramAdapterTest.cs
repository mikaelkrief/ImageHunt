using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImagehuntBotBuilder;
using ImageHuntBotBuilder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class TelegramAdapterTest : BaseTest
    {
        private TelegramAdapter _target;
        private IMapper _mapper;
        private ILogger<TelegramAdapter> _logger;
        private ITelegramBotClient _telegramBotClient;

        public TelegramAdapterTest()
        {
            Startup.ConfigureMappings();
            _testContainerBuilder.RegisterType<TelegramAdapter>();
            _logger = A.Fake<ILogger<TelegramAdapter>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _mapper = Mapper.Instance;
            _testContainerBuilder.RegisterInstance(_mapper);
            _telegramBotClient = A.Fake<ITelegramBotClient>();
            _testContainerBuilder.RegisterInstance(_telegramBotClient);
            var container = _testContainerBuilder.Build();
            _target = container.Resolve<TelegramAdapter>();
        }

        [Fact]
        public void Should_Map_Update_to_Activity()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.sendText.json")
                    .ToObject<Update>();
            var mapper = Mapper.Instance;
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
        public void Should_Map_Update_to_Activity_Location()
        {
            // Arrange
            var update =
                GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.sendLocation.json")
                    .ToObject<Update>();
            var mapper = Mapper.Instance;
            // Act
            var activity = _mapper.Map<Activity>(update);
            // Assert
            Check.That(activity.Id).Equals(update.EditedMessage.Chat.Id.ToString());
            Check.That(activity.Type).Equals("location");
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
            string authHeader = "Auth";
            var callback = A.Fake<BotCallbackHandler>();
            Activity activity = new Activity()
            {
                Properties = GetJObjectFromResource(Assembly.GetExecutingAssembly(), "ImageHuntBotBuilderTest.Data.sendImage.json")
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
            string authHeader = "Auth";
            var callback = A.Fake<BotCallbackHandler>();

            Activity activity = new Activity() {Type = "message", ServiceUrl = "http://localhost"};

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
            var turnState = new TurnContextStateCollection(){ {typeof(ConnectorClient).FullName,connectorClient} };
            A.CallTo(() => turnContext.TurnState).Returns(turnState);
            A.CallTo(() => conversation.SendToConversationWithHttpMessagesAsync(A<string>._, A<Activity>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(new HttpOperationResponse<ResourceResponse>());
            var activities = new Activity[] {new Activity(){Type = "message", ChannelId = "emulator", Id = "51515115",
                Conversation = new ConversationAccount(), Text = "toto"},
                
            };
            // Act
            var responses = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            Check.That(responses).HasSize(activities.Length);
            Check.That(responses.Extracting("Id")).Contains("51515115");
        }
        [Fact]
        public async Task Should_SendActivitiesAsync_on_telegram_return()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var connectorClient = A.Fake<IConnectorClient>();
            var conversation = A.Fake<IConversations>();
            A.CallTo(() => connectorClient.Conversations).Returns(conversation);
            var turnState = new TurnContextStateCollection(){ {typeof(IConnectorClient).FullName,connectorClient} };
            A.CallTo(() => turnContext.TurnState).Returns(turnState);
            A.CallTo(() => conversation.SendToConversationWithHttpMessagesAsync(A<string>._, A<Activity>._, A<Dictionary<string, List<string>>>._, A<CancellationToken>._))
                .Returns(new HttpOperationResponse<ResourceResponse>());
            var activities = new Activity[] {new Activity(){Type = "message", ChannelId = "telegram", Id = "151515",
                Conversation = new ConversationAccount(), Text = "toto"},
                
            };
            // Act
            var responses = await _target.SendActivitiesAsync(turnContext, activities, CancellationToken.None);
            // Assert
            Check.That(responses).HasSize(activities.Length);
            A.CallTo(() => _telegramBotClient.SendTextMessageAsync(A<ChatId>._, "toto", A<ParseMode>._, A<bool>._,
                A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}
