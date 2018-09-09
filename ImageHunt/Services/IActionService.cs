using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
    Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int take);
    GameAction GetGameAction(int gameActionId);
    void Validate(int gameActionId, int validatorId);
    int GetGameActionCountForGame(int gameId);
    IEnumerable<Score> GetScoresForGame(int gameId);
    IEnumerable<GameAction> GetGamePositionsForGame(int gameId);
  }
}
