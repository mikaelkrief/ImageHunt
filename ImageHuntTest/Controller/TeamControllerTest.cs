using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class TeamControllerTest
    {
        private ITeamService _teamService;
        private TeamController _target;

        public TeamControllerTest()
        {
            _teamService = A.Fake<ITeamService>();
            _target = new TeamController(_teamService);
        }

        [Fact]
        public void Get()
        {
            // Arrange
            
            // Act
            var result = _target.GetTeam(1);
            // Assert
        }

        [Fact]
        public void AddPlayer()
        {
            // Arrange
            var team = new Team();
            A.CallTo(() => _teamService.GetTeamById(A<int>._)).Returns(team);
            var player = new Player(){Name = "toto", ChatLogin = "Toro"};
            // Act
            _target.AddPlayer(1, player);
            // Assert
            A.CallTo(() => _teamService.AddMemberToTeam(team, A<List<Player>>._)).MustHaveHappened();
        }

      [Fact]
      public void GetPlayerDetails()
      {
        // Arrange
        string playerLogin = "Toto";
        var player = new Player(){};
        int gameId = 1;
        A.CallTo(() => _teamService.GetPlayer(playerLogin, gameId)).Returns(player);
        // Act
        _target.GetPlayer(gameId, playerLogin);
        // Assert
        A.CallTo(() => _teamService.GetPlayer(playerLogin, gameId)).MustHaveHappened();
      }
    }
}
