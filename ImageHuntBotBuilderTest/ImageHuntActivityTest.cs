using System;
using System.Collections.Generic;
using System.Text;
using ImageHuntBotBuilder;
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
    }
}