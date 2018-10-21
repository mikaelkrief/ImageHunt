using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Services
{
  public interface IActionService : IService
  {
    void AddGameAction(GameAction gameAction);
    Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int pageSize, int take, IncludeAction includeAction, int? teamId = null);
    GameAction GetGameAction(int gameActionId);
    void Validate(int gameActionId, int validatorId, bool validate);
    int GetGameActionCountForGame(int gameId, IncludeAction includeAction, int? teamId = null);
    IEnumerable<Score> GetScoresForGame(int gameId);
    IEnumerable<GameAction> GetGamePositionsForGame(int gameId);
  }
}
