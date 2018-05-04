using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot.WebServices;
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
      FakeResponse("ImageHuntBotTest.Data.TeamById1.json");

      // Act
      var response = await _target.GetTeamById(3);
      // Assert
      Check.That(response.Id).Equals(3);
      Check.That(response.Name).Equals("Team 1");
      Check.That(response.Players.Extracting("Id")).Contains(1, 2);
      Check.That(response.Players.Extracting("Name")).Contains("player1", "player2");
      Check.That(response.Players.Extracting("ChatLogin")).Contains("player1", "player2");
    }
  }
}
