using System.Collections.Generic;
using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
    IEnumerable<GameAction> GetGameActionsForGame(int gameId);
    GameAction GetGameAction(int gameActionId);
    void Validate(int gameActionId);
  }
}
