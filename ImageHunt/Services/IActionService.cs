using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHuntCore.Model;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
    Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int pageSize, IncludeAction includeAction, int? teamId = null);
    GameAction GetGameAction(int gameActionId);
    GameAction Validate(int actionId, int gameActionId, int validatorId, bool validate);
    int GetGameActionCountForGame(int gameId, IncludeAction includeAction, int? teamId = null);
    IEnumerable<Score> GetScoresForGame(int gameId);
    IEnumerable<GameAction> GetGamePositionsForGame(int gameId);
  }
}
