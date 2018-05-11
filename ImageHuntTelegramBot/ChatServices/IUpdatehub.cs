using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Services
{
  public interface IUpdateHub
    {
      Task Switch(Update update);
    }
}
