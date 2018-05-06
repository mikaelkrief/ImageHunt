using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Responses;

namespace ImageHuntTelegramBot.WebServices
{
  public class TeamWebService : AbstractWebService, ITeamWebService
  {
    public TeamWebService(HttpClient httpClient) : base(httpClient)
    {
      
    }


    public async Task<TeamResponse> GetTeamById(int teamId)
    {
      return await GetAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/{teamId}");
    }

}
}