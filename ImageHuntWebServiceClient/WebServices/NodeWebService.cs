using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public class NodeWebService : AbstractWebService, INodeWebService
    {
        public NodeWebService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<NodeResponse> GetNode(int nodeId)
        {
            return await GetAsync<NodeResponse>($"{_httpClient.BaseAddress}api/Node/{nodeId}");
        }
    }
}