using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot
{
  public interface IAdapter
  {
    Task SendActivity(IActivity activity);
    Task<Activity> CreateActivityFromUpdate(Update update);
      Task SetWebHook();
        Task Leave(ChatId chatId);
    }
}