using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient;
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
            var logPositionRequest = new LogPositionRequest()
            {
                GameId = 1,
                TeamId = 1,
                Latitude = 45.26,
                Longitude = 4.63
            };
            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };

            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "PostAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(httpResponse);
            // Act
            await _target.LogPosition(logPositionRequest);
            // Assert
        }
        [Fact]
        public async Task LogAction()
        {
            // Arrange
            var logActionRequest = new GameActionRequest()
            {
                GameId = 1,
                TeamId = 1,
                Latitude = 45.26,
                Longitude = 4.63,
                Action = (int)Action.StartGame.GetTypeCode()
            };

            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };

            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "PostAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(httpResponse);
            // Act
            await _target.LogAction(logActionRequest);
            // Assert
            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>().MustHaveHappened();

        }
        [Fact]
        public async Task LogAction_LogGivePoints()
        {
            // Arrange
            var logActionRequest = new GameActionRequest()
            {
                GameId = 1,
                TeamId = 1,
                Action = (int)Action.GivePoints
            };

            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };

            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "PostAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(httpResponse);
            // Act
            await _target.LogAction(logActionRequest);
            // Assert
            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>().MustHaveHappened();

        }
    }
}
