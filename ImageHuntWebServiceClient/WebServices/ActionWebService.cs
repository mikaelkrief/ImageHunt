using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;

namespace ImageHuntWebServiceClient.WebServices
{
    public class ActionWebService : AbstractWebService, IActionWebService
    {
        public ActionWebService(HttpClient httpClient, ILogger<IActionWebService> logger) : base(httpClient, logger)
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
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(logActionRequest.Action.ToString()), "action");
                content.Add(new StringContent(logActionRequest.GameId.ToString()), "gameId");
                content.Add(new StringContent(logActionRequest.TeamId.ToString()), "teamId");
                content.Add(new StringContent(logActionRequest.NodeId.ToString()), "nodeId");
                content.Add(new StringContent(logActionRequest.Latitude.ToString()), "latitude");
                content.Add(new StringContent(logActionRequest.Longitude.ToString()), "longitude");
                content.Add(new StringContent(logActionRequest.PointsEarned.ToString()), "pointsEarned");
                content.Add(new StringContent(logActionRequest.PictureId.ToString()), "pictureId");

                var result = await PostAsync<GameActionResponse>($"{_httpClient.BaseAddress}api/Action/AddGameAction/",
                    content, cancellationToken);
                 return result;
            }
        }
    }
}