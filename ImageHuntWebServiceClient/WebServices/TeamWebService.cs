using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
  public class TeamWebService : AbstractWebService, ITeamWebService
  {
    public TeamWebService(HttpClient httpClient, ILogger<ITeamWebService> logger) : base(httpClient, logger)
    {
      
    }
      public async Task<NodeResponse> StartGameForTeam(int gameId, int teamId,
          CancellationToken cancellationToken = default(CancellationToken))
      {
          var result = await PutAsync<NodeResponse>($"{_httpClient.BaseAddress}api/Team/StartTeam/{gameId}/{teamId}", cancellationToken);
          return result;
      }


        public async Task<TeamResponse> GetTeamById(int teamId)
    {
      return await GetAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/{teamId}");
    }

    public async Task<NodeResponse> UploadImage(UploadImageRequest uploadImageRequest)
    {


      using (var content = new MultipartFormDataContent())
      {
        content.Add(new StringContent(uploadImageRequest.GameId.ToString()), "gameId");
        content.Add(new StringContent(uploadImageRequest.TeamId.ToString()), "teamId");
        content.Add(new StringContent(uploadImageRequest.Latitude.ToString()), "latitude");
        content.Add(new StringContent(uploadImageRequest.Longitude.ToString()), "longitude");
        using (var fileStream = uploadImageRequest.FormFile.OpenReadStream())
        {
          content.Add(new StreamContent(fileStream), "formFile", "image.jpg");
          return await PostAsync<NodeResponse>($"{_httpClient.BaseAddress}api/Team/UploadImage/", content);
        }
      }
        
    }

      public async Task<TeamResponse> GetTeamForUserName(int gameId, string userName)
      {
          return await GetAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/GetTeamsOfPlayer/{gameId}/{userName}");
      }

      public async Task<TeamResponse> CreateTeam(int gameId, TeamRequest teamRequest)
      {
          using (var content = new StringContent(JsonConvert.SerializeObject(teamRequest), Encoding.UTF8, "application/json"))
          {
              return await PostAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/{gameId}", content);
          }

      }

      public async Task<TeamResponse> AddPlayer(int teamId, PlayerRequest playerRequest)
      {
          using (var content = new StringContent(JsonConvert.SerializeObject(playerRequest), Encoding.UTF8, "application/json"))
          {
              return await PostAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/AddPlayer/{teamId}", content);
          }
      }

      public async Task RemovePlayerFromTeam(int teamId, string chatId)
      {
          await DeleteAsync($"{_httpClient.BaseAddress}api/Team/RemoveByChatId/{teamId}/{chatId}");
      }
  }
}