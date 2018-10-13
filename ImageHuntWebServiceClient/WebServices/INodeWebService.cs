using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface INodeWebService
    {
        Task<NodeResponse> GetNode(int nodeId);
    }
}
