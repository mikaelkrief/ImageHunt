using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
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