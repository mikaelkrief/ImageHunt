using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntBotTest.WebServices;
using ImageHuntTelegramBot.WebServices;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class GameServiceTest : WebServiceBaseTest
    {
      private GameWebService _target;

      public GameServiceTest()
      {
        _target = new GameWebService(_httpClient);
      }
      [Fact]
      public async Task GetGameById()
      {
      // Arrange
        FakeResponse("ImageHuntBotTest.Data.GameById1.json");

      
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
