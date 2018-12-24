using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface INodeWebService
    {
        Task<NodeResponse> GetNode(int nodeId);
        Task<IEnumerable<NodeResponse>> GetNodesByType(NodeTypes nodeType, int gameId);
    }
}
