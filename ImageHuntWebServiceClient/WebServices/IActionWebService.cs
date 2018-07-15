using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IActionWebService
    {
        Task LogPosition(LogPositionRequest logPositionRequest, CancellationToken cancellationToken = default(CancellationToken));
    }
}