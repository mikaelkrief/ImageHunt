using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Services;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class PasscodeWebServiceTest : WebServiceBaseTest
    {
        private PasscodeWebService _target;
        private ILogger<PasscodeWebService> _logger;

        public PasscodeWebServiceTest()
        {
            _logger = A.Fake<ILogger<PasscodeWebService>>();
            _target = new PasscodeWebService(_httpClient, _logger);
        }

        [Fact]
        public async Task RedeemPasscode()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };

            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "PatchAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(httpResponse);

            // Act
            await _target.RedeemPasscode(1, "toto", "HJHJHJH");
            // Assert
        }
    }
}
