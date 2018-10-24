using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
  public class GameWebService : AbstractWebService, IGameWebService
  {
    public async Task<GameResponse> GetGameById(int gameId, CancellationToken cancellationToken=default(CancellationToken))
    {
      return await GetAsync<GameResponse>($"{_httpClient.BaseAddress}api/Game/ById/{gameId}", cancellationToken);
    }

    public GameWebService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<NodeResponse> StartGameForTeam(int gameId, int teamId,
      CancellationToken cancellationToken = default(CancellationToken))
    {
      var result = await PutAsync<NodeResponse>($"{_httpClient.BaseAddress}api/Team/StartTeam/{gameId}/{teamId}", cancellationToken);
      return result;
    }

      public async Task<IEnumerable<ScoreResponse>> GetScoresForGame(int gameId, CancellationToken cancellationToken = default(CancellationToken))
      {
          var result = await GetAsync<IEnumerable<ScoreResponse>>($"{_httpClient.BaseAddress}api/Game/Score/{gameId}",
              cancellationToken);
          return result;
      }

      public async Task<IEnumerable<NodeResponse>> GetPictureNodesForGame(int gameId, CancellationToken cancellationToken = default(CancellationToken))
      {
          var result =
              await GetAsync<IEnumerable<NodeResponse>>($"{_httpClient.BaseAddress}api/Game/GetImages/{gameId}",
                  cancellationToken);
          return result;
      }
  }
}