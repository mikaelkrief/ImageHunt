using System.Threading.Tasks;
using ImageHuntTelegramBot.Responses;

namespace ImageHuntTelegramBot.WebServices
{
  public interface IGameWebService
  {
    Task<GameResponse> GetGameById(int gameId);
    Task<NodeResponse> StartGameForTeam(int gameId, int teamId);
  }
}