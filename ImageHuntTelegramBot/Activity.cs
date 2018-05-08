namespace ImageHuntTelegramBot
{
    public class Activity : IActivity
    {
      public ActivityType ActivityType { get; set; }
      public string Text { get; set; }
      public long ChatId { get; set; }
    }

  public enum ActivityType
  {
    None,
    Message,
    UpdateMessage,
    CallbackQuery

  }

  public interface IActivity
  {
    ActivityType ActivityType { get; set; }
    string Text { get; set; }
    long ChatId { get; set; }
  }
}
