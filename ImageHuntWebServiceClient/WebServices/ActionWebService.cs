﻿using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
    public class ActionWebService : AbstractWebService, IActionWebService
    {
        public ActionWebService(HttpClient httpClient) : base(httpClient)
        {
            
        }

        public async Task LogPosition(LogPositionRequest logPositionRequest, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(logPositionRequest.GameId.ToString()), "gameId");
                content.Add(new StringContent(logPositionRequest.TeamId.ToString()), "teamId");
                content.Add(new StringContent(logPositionRequest.Latitude.ToString()), "latitude");
                content.Add(new StringContent(logPositionRequest.Longitude.ToString()), "longitude");
                var result = await PostAsync<string>($"{_httpClient.BaseAddress}api/Action/LogPosition/",
                    content, cancellationToken);
            }
        }

        public async Task<GameActionResponse> LogAction(GameActionRequest logActionRequest, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(logActionRequest), Encoding.UTF8, "application/json"))
            {
                var result = await PostAsync<GameActionResponse>($"{_httpClient.BaseAddress}api/Action/AddGameAction/",
                    content, cancellationToken);
                 return result;
            }
        }
    }
}