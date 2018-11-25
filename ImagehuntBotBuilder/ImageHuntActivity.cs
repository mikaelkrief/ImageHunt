using Microsoft.Bot.Schema;

namespace ImageHuntBotBuilder
{
    public class ImageHuntActivity : Activity
    {
        public new Activity ApplyConversationReference(ConversationReference reference, bool isIncoming = false)
        {
            this.ChannelId = reference.ChannelId;
            this.ServiceUrl = reference.ServiceUrl;
            if (this.Conversation == null)
                this.Conversation = reference.Conversation;

            if (isIncoming)
            {
                if (this.From == null)
                    this.From = reference.User;
                if (this.Recipient == null)
                    this.Recipient = reference.Bot;
                if (reference.ActivityId != null)
                    this.Id = reference.ActivityId;
            }
            else // Outgoing
            {
                if (this.From == null)

                    this.From = reference.Bot;
                if (this.Recipient == null)

                    this.Recipient = reference.User;
                if (reference.ActivityId != null)
                    this.ReplyToId = reference.ActivityId;
            }

            return this;
        }
    }
}