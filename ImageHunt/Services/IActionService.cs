using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
  }
}
