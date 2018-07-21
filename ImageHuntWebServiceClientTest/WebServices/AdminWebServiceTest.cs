using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ImageHuntWebServiceClient.WebServices;
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
        public void GetAllAdmins()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }

    public class AdminWebService : AbstractWebService, IAdminSebService
    {
        public AdminWebService(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
