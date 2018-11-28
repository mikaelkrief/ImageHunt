using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntBotBuilder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class ImageHuntActivityTest : BaseTest<ImageHuntActivity>
    {
        public ImageHuntActivityTest()
        {
            Build();
        }

        [Fact]
        public void Should_Set_Conversation_If_Null()
        {
            // Arrange
            var conversatonReference = new ConversationReference(){Conversation = new ConversationAccount()};
            // Act
            _target.ApplyConversationReference(conversatonReference);
            // Assert
            Check.That(_target.Conversation).Equals(conversatonReference.Conversation);

        }
        [Fact]
        public void Should_Not_Set_Conversation_If_Not_Null()
        {
            // Arrange
            _target.Conversation = new ConversationAccount();
            var conversatonReference = new ConversationReference(){Conversation = new ConversationAccount()};
            // Act
            _target.ApplyConversationReference(conversatonReference);
            // Assert
            Check.That(_target.Conversation).IsNotEqualTo(conversatonReference.Conversation);
        }

        [Fact]
        public async Task Should_TurnContext_Not_Modify_Conversation_if_already_set()
        {
            // Arrange
            var adapter = A.Fake<BotAdapter>();
            var orgActivity = new ImageHuntActivity(){Conversation = new ConversationAccount(id: "ConvId1")};
            var turnContext = new TurnContext(adapter, orgActivity);
            var newActivity = new ImageHuntActivity() {Conversation = new ConversationAccount(id:"Conv2")};
            // Act
            await turnContext.SendActivitiesAsync(new[] {newActivity});
            // Assert
            Check.That(newActivity.Conversation.Id).Equals("Conv2");
        }
    }
}