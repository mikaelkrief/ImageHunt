using System;
using System.Text;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.WebServices;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class AdminWebServiceTest : WebServiceBaseTest
    {
        private AdminWebService _target;

        public AdminWebServiceTest()
        {
            _target = new AdminWebService(_httpClient);
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
