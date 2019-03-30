using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class AdminWebServiceTest : WebServiceBaseTest
    {
        private AdminWebService _target;
        private ILogger<IAdminWebService> _logger;

        public AdminWebServiceTest()
        {
            _logger = A.Fake<ILogger<IAdminWebService>>();
            _target = new AdminWebService(_httpClient, _logger);
        }
        [Fact]
        public async Task GetAllAdmins()
        {
            // Arrange
            FakeResponse("ImageHuntWebServiceClientTest.Data.GetAllAdmin.json");
            // Act
            var results = await _target.GetAllAdmins();
            // Assert
            Check.That(results).HasSize(2);
            Check.That(results.Extracting("Name")).Contains("Toto", "ImageHuntBot");
        }
    }
}
