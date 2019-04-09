using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class NodeWebServiceTest : WebServiceBaseTest
    {
        private NodeWebService _target;
        private ILogger<INodeWebService> _logger;

        public NodeWebServiceTest()
        {
            _logger = A.Fake<ILogger<INodeWebService>>();
            _target = new NodeWebService(HttpClient, _logger);

        }

        [Fact]
        public async Task GetNode()
        {
            // Arrange

            // Act
            await _target.GetNode(15);
            // Assert
        }

        [Fact]
        public async Task GetNodesbyTypes()
        {
            // Arrange
            
            // Act
            await _target.GetNodesByType(NodeTypes.Hidden, 1);
            // Assert
        }
    }
}
