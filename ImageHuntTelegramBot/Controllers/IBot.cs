using System.Threading.Tasks;

namespace ImageHuntTelegramBot.Controllers
{
  public interface IBot
  {
    Task OnTurn(ITurnContext context);
  }
}