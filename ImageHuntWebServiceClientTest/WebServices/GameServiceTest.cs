using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class GameServiceTest : WebServiceBaseTest
    {
      private GameWebService _target;
        private ILogger<IGameWebService> _logger;

        public GameServiceTest()
      {
          _logger = A.Fake<ILogger<IGameWebService>>();
        _target = new GameWebService(_httpClient, _logger);
      }
      [Fact]
      public async Task GetGameById()
      {
      // Arrange
        FakeResponse("ImageHuntWebServiceClientTest.Data.GameById1.json");

      
        // Act
        var response = await _target.GetGameById(1);
        // Assert
        A.CallTo(_fakeHttpMessageHandler)
          .Where(x=>x.Method.Name == "SendAsync")
          .WithReturnType<Task<HttpResponseMessage>>()
          .MustHaveHappened();
        Check.That(response.Id).Equals(1);
        Check.That(response.Name).Equals("Paris");
      }


      public class DummyHttpMessageHandler : HttpMessageHandler
      {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
          await Task.Delay(1000000000, cancellationToken);
          return new HttpResponseMessage();
        }
      }
      //[Fact]
      //public async Task StartGameWithCancellation()
      //{
      //// Arrange
      //  HttpResponseMessage responseMessage = new HttpResponseMessage()
      //  {
      //    Content = new StringContent(GetStringFromResource(Assembly.GetExecutingAssembly(),
      //      "ImageHuntWebServiceClientTest.Data.StartTeamFirstNode.json"
      //    ))
      //  };

      //_fakeHttpMessageHandler = A.Fake<DummyHttpMessageHandler>();
      //  _httpClient = A.Fake<HttpClient>();
      //  A.CallTo(() => _httpClient.PutAsync(A<Uri>._, A<HttpContent>._)).Invokes(() => Task.Delay(1500000)).Returns(responseMessage);
      //  _target = new GameWebService(_httpClient);
      //var cancellationToken = default(CancellationToken);
      //  // Act
      //  var result = await _target.StartGameForTeam(1, 2, cancellationToken);
      //  // Assert
      //}
  }
}
