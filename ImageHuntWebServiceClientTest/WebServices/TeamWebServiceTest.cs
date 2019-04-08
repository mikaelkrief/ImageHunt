using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.WebServices
{
    public class TeamWebServiceTest : WebServiceBaseTest
    {
        private TeamWebService _target;
        private ILogger<ITeamWebService> _logger;

        public TeamWebServiceTest()
        {
            _logger = A.Fake<ILogger<ITeamWebService>>();
            _target = new TeamWebService(_httpClient, _logger);
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
        public async Task StartGame()
        {
            // Arrange
            FakeResponse("ImageHuntWebServiceClientTest.Data.StartTeamFirstNode.json");
            // Act
            var result = await _target.StartGameForTeam(1, 2);
            // Assert
            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .MustHaveHappened();
            Check.That(result.Id).Equals(1);
            Check.That(result.Name).Equals("Départ");
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

        [Fact]
        public async Task CreateTeamAsync()
        {
            // Arrange
            FakeResponse("ImageHuntWebServiceClientTest.Data.createTeam.json");

            var teamRequest = new TeamRequest() { Name = "Team1", ChatId = "151515", Color = "0x525515" };
            // Act
            var response = await _target.CreateTeam(1, teamRequest);
            // Assert
            Check.That(response.Name).Equals(teamRequest.Name);
            Check.That(response.ChatId).Equals(teamRequest.ChatId);
        }

        [Fact]
        public async Task Should_GetPlayer_Return_Player()
        {
            // Arrange
            
            // Act
            await _target.RemovePlayerFromTeam(1, "toto");
            // Assert
        }

        [Fact]
        public async Task Should_UpdateTeam_Succeed()
        {
            // Arrange
            var updateRequest = new UpdateTeamRequest();
            // Act
            await _target.UpdateTeam(updateRequest);
            // Assert
        }
    }
}
