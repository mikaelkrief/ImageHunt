using System.Threading.Tasks;
using ImageHuntTelegramBot.Responses;

namespace ImageHuntTelegramBot.WebServices
{
  public interface IGameWebService
  {
    Task<GameResponse> GetGameById(int gameId);
  }
}