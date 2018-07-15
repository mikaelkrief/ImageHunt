using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class ActionWebServiceTest : WebServiceBaseTest
    {
        private ActionWebService _target;

        public ActionWebServiceTest()
        {
            _target = new ActionWebService(_httpClient);
        }

        [Fact]
        public async Task LogPosition()
        {
            // Arrange
            var logPositionRequest = new LogPositionRequest();
            // Act
            await _target.LogPosition(logPositionRequest);
            // Assert
        }
    }
}
