using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public interface IAdapter
  {
    Task SendActivity(IActivity activity);
  }
}