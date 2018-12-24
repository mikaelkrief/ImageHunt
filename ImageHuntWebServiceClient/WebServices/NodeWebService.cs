using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;

namespace ImageHuntWebServiceClient.WebServices
{
    public class NodeWebService : AbstractWebService, INodeWebService
    {
        public NodeWebService(HttpClient httpClient, ILogger<INodeWebService> logger) : base(httpClient, logger)
        {
        }

        public async Task<NodeResponse> GetNode(int nodeId)
        {
            return await GetAsync<NodeResponse>($"{_httpClient.BaseAddress}api/Node/{nodeId}");
        }

        public async Task<IEnumerable<NodeResponse>> GetNodesByType(NodeTypes nodeType, int gameId)
        {
            return await GetAsync<IEnumerable<NodeResponse>>(
                $"{_httpClient.BaseAddress}api/Node/GetNodesByType/{gameId}/{nodeType}");
        }
    }
}