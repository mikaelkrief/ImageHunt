using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Http.Internal;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.WebServices
{
  public class TeamServiceTest : WebServiceBaseTest
  {
    private TeamWebService _target;

    public TeamServiceTest()
    {
      _target = new TeamWebService(_httpClient);
    }
    [Fact]
    public async Task GetTeamById()
    {
      // Arrange
      FakeResponse("ImageHuntWebServiceClientTest.Data.TeamById1.json");

      // Act
      var response = await _target.GetTeamById(3);
      // Assert
      Check.That(response.Id).Equals(3);
      Check.That(response.Name).Equals("Team 1");
      Check.That(response.Players.Extracting("Id")).Contains(1, 2);
      Check.That(response.Players.Extracting("Name")).Contains("player1", "player2");
      Check.That(response.Players.Extracting("ChatLogin")).Contains("player1", "player2");
    }

    [Fact]
    public async Task UploadImage()
    {
      // Arrange
      byte[] data = new byte[15];
      FakeResponse("ImageHuntWebServiceClientTest.Data.StartTeamFirstNode.json");
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("toto")))
      {
        var uploadrequest = new UploadImageRequest()
        {
          FormFile = new FormFile(stream, 0, stream.Length, "formFile", "toto.txt")
        };
        var response = await _target.UploadImage(uploadrequest);
      }

      // Act
      // Assert
      //A.CallTo(() => _fakeHttpMessageHandler.(A<string>._, A<HttpContent>._, A<CancellationToken>._)).MustHaveHappened();

    }
  }
}
