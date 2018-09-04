﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Newtonsoft.Json;

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

      public async Task<TeamResponse> GetTeamForUserName(string userName)
      {
          return await GetAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/GetTeamsOfPlayer/{userName}");
      }
  }
}