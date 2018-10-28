using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public interface IDialog
  {
    Task Begin(ITurnContext turnContext, bool overrideAdmin = false);
    Task Continue(ITurnContext turnContext);
    void AddChildren(IDialog childrenDialog);
    Task Reply(ITurnContext turnContext);
    string Command { get; }
      bool IsAdmin { get; }
  }
}