using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IActionWebService
    {
        Task LogPosition(LogPositionRequest logPositionRequest, CancellationToken cancellationToken = default(CancellationToken));

        Task<GameActionResponse> LogAction(GameActionRequest logActionRequest,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}