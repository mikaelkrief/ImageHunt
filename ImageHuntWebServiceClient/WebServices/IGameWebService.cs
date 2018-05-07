using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
  public interface IGameWebService
  {
    Task<GameResponse> GetGameById(int gameId);
    Task<NodeResponse> StartGameForTeam(int gameId, int teamId);
  }
}