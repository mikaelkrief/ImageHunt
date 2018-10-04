using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.WebServices;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class NodeWebServiceTest : WebServiceBaseTest
    {
        private NodeWebService _target;

        public NodeWebServiceTest()
        {
            _target = new NodeWebService(_httpClient);

        }

        [Fact]
        public async Task GetNode()
        {
            // Arrange

            // Act
            await _target.GetNode(15);
            // Assert
        }
    }
}
