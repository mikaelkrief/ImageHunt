using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
    Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int take);
    GameAction GetGameAction(int gameActionId);
    void Validate(int gameActionId, int validatorId);
  }
}
