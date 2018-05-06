using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Responses;

namespace ImageHuntTelegramBot.WebServices
{
  public interface ITeamWebService
  {
    Task<TeamResponse> GetTeamById(int teamId);
  }
}