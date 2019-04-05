using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
  public interface IGameWebService
  {
    Task<GameResponseEx> GetGameById(int gameId, CancellationToken cancellationToken = default (CancellationToken));
      Task<IEnumerable<ScoreResponse>> GetScoresForGame(int gameId, CancellationToken cancellationToken = default(CancellationToken));

      Task<IEnumerable<NodeResponse>> GetPictureNodesForGame(int gameId,
          CancellationToken cancellationToken = default(CancellationToken));

  }
}
