using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class AccountWebServiceTest : WebServiceBaseTest
    {
        private ILogger<IAccountWebService> _logger;
        private AccountWebService _target;

        public AccountWebServiceTest()
        {
            _logger = A.Fake<ILogger<IAccountWebService>>();
            _target = new AccountWebService(_httpClient, _logger);
        }

        [Fact]
        public async Task Should_Login_As_Bot()
        {
            // Arrange
            var loginRequest = new LoginRequest()
            {
                UserName = "ImageHuntDevBot",
                Password = "G5HFugèGyGF453;"
            };
            FakeResponse("ImageHuntWebServiceClientTest.Data.login_as_bot.json");

            // Act
            var result = await _target.Login(loginRequest);
            // Assert
            Check.That(result.Value).IsNotNull();
        }
    }
}
