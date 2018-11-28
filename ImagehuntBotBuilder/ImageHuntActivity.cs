using Microsoft.Bot.Schema;

namespace ImageHuntBotBuilder
{
    public class ImageHuntActivity : Activity, IActivity
    {
        public new Activity ApplyConversationReference(ConversationReference reference, bool isIncoming = false)
        {
            ChannelId = reference.ChannelId;
            ServiceUrl = reference.ServiceUrl;
            if (Conversation == null)
                Conversation = reference.Conversation;

            if (isIncoming)
            {
                if (From == null)
                    From = reference.User;
                if (Recipient == null)
                    Recipient = reference.Bot;
                if (reference.ActivityId != null)
                    Id = reference.ActivityId;
            }
            else // Outgoing
            {
                if (From == null)

                    From = reference.Bot;
                if (Recipient == null)

                    Recipient = reference.User;
                if (reference.ActivityId != null)
                    ReplyToId = reference.ActivityId;
            }

            return this;
        }
    }
}