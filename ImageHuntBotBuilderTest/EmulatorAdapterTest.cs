using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.TransientFaultHandling;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class EmulatorAdapterTest : BaseTest<EmulatorAdapter>
    {
        private object _credentialProvider;
        private IChannelProvider _channelProvider;
        private RetryPolicy _retryPolicy;
        private BotCallbackHandler _botCallback;

        public EmulatorAdapterTest()
        {
            _botCallback = A.Fake<BotCallbackHandler>();
            Build();
        }

        [Fact]
        public async Task Should_Transform_location_command()
        {
            // Arrange
            var activity = new Activity() {Type = ActivityTypes.Message, Text = "/location lat=45.6 lng=5.77"};
            // Act
            await _target.ProcessActivityAsync(null, activity, _botCallback, CancellationToken.None);
            // Assert
            A.CallTo(() => _botCallback.Invoke(A<ITurnContext>.That.Matches(t=>CheckLocation(t)), A<CancellationToken>._)).MustHaveHappened();
        }

        private bool CheckLocation(ITurnContext turnContext)
        {
            Check.That(turnContext.Activity.Type).Equals(ImageHuntActivityTypes.Location);
            Check.That(turnContext.Activity.Attachments.First().ContentType).Equals(ImageHuntActivityTypes.Location);
            Check.That(turnContext.Activity.Attachments.First().Content).IsInstanceOf<GeoCoordinates>();
            return true;
        }

        [Fact]
        public async Task Should_SendActivities()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var turnContextStateCollection = new TurnContextStateCollection();
            var connectorClient = A.Fake<IConnectorClient>();
            IConversations conversations = A.Fake<IConversations>();
            A.CallTo(() => connectorClient.Conversations).Returns(conversations);
            turnContextStateCollection.Add(typeof(IConnectorClient).FullName, connectorClient);
            A.CallTo(() => turnContext.TurnState).Returns(turnContextStateCollection);
            // Act
            var activities = new List<Activity>()
            {
                new Activity(replyToId:"hjh", conversation:new ConversationAccount(id:"ghghg"))
            };
            await _target.SendActivitiesAsync(turnContext, activities.ToArray(), CancellationToken.None);
            // Assert
            A.CallTo(() => conversations.ReplyToActivityWithHttpMessagesAsync(A<string>._, A<string>._, A<Activity>._,
                A<Dictionary<string, List<string>>>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}