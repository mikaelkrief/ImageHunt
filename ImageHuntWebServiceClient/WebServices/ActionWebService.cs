using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Mvc;

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
                var result = await PostAsync<IActionResult>($"{_httpClient.BaseAddress}api/Action/LogPosition/",
                    content, cancellationToken);
            }
        }

        public async Task LogAction(GameActionRequest logActionRequest, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(logActionRequest.GameId.ToString()), "gameId");
                content.Add(new StringContent(logActionRequest.TeamId.ToString()), "teamId");
                content.Add(new StringContent(logActionRequest.Latitude.ToString()), "latitude");
                content.Add(new StringContent(logActionRequest.Longitude.ToString()), "longitude");
                content.Add(new StringContent(logActionRequest.Action.ToString()), "action");
                var result = await PostAsync<IActionResult>($"{_httpClient.BaseAddress}api/Action/AddGameAction/",
                    content, cancellationToken);
            }
        }
    }
}