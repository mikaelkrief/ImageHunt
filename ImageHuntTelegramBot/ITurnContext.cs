using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
    public interface ITurnContext
    {
      IActivity Activity { get; set; }
      string ChatId { get; set; }
      bool Replied { get; }

      Task Continue();
      Task End();
      Task ReplyActivity(IActivity activity);
    }
}
