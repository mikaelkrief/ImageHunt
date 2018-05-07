using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public interface IDialog
  {
    Task Begin(ITurnContext turnContext);
    Task Continue(ITurnContext turnContext);
    Task OnTurn(ITurnContext turnContext);
    void AddChildren(IDialog childrenDialog);
    Task Reply(ITurnContext turnContext);
  }
}