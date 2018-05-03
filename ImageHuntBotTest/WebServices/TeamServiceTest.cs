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
      var response = await _target.GetTeamById(1);
        // Assert
        Check.That(response.Id).Equals(1);
        Check.That(response.Name).Equals("Team 1");
      }
    }

  public class TeamWebService : AbstractWebService, ITeamWebService
  {
    public TeamWebService(HttpClient httpClient) : base(httpClient)
    {
      
    }


    public async Task<TeamResponse> GetTeamById(int teamId)
    {
      return await GetAsync<TeamResponse>($"{_httpClient.BaseAddress}api/Team/{teamId}");
    }
  }

  public class TeamResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
