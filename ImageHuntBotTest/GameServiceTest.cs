using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.WebServices;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class GameServiceTest : BaseTest
    {
      private GameWebService _target;
      private HttpMessageHandler _fakeHttpMessageHandler;
      private HttpClient _httpClient;

      public GameServiceTest()
      {
        _fakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
        _httpClient = new HttpClient(_fakeHttpMessageHandler){BaseAddress = new Uri("http://test.com")};
        _target = new GameWebService(_httpClient);
      }
      [Fact]
      public async Task GetGameById()
      {
      // Arrange
        var httpResponse = new HttpResponseMessage
        {
          Content = new StringContent(GetStringFromResource(Assembly.GetExecutingAssembly(),
            "ImageHuntBotTest.Data.GameById1.json"))
        };
        A.CallTo(_fakeHttpMessageHandler)
          .Where(x => x.Method.Name == "SendAsync")
          .WithReturnType<Task<HttpResponseMessage>>()
          .Returns(httpResponse);

      
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
    }
}
