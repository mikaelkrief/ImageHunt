using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ImageHuntTelegramBot
{
  public class TelegramAdapter : IAdapter
  {
    private readonly ITelegramBotClient _client;

    public TelegramAdapter(ITelegramBotClient client)
    {
      _client = client;
    }
    public async Task SendActivity(IActivity activity)
    {
      switch (activity.ActivityType)
      {
        case ActivityType.Message:
          await SendMessage(activity);
          break;
        case ActivityType.UpdateMessage:
          break;
        case ActivityType.CallbackQuery:
          break;
        case ActivityType.None:
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private async Task SendMessage(IActivity activity)
    {
      await _client.SendTextMessageAsync(activity.ChatId, activity.Text);
    }
  }
}
