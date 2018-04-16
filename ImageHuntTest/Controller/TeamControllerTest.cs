using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using NFluent;
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
      public void CreateTeam()
      {
        // Arrange
        var team = new Team() {Name = "Team"};
        
        // Act
        _target.CreateTeam(1, team);
        // Assert
        A.CallTo(() => _teamService.CreateTeam(1, team)).MustHaveHappened();
      }

      [Fact]
      public void DeleteTeam()
      {
        // Arrange
        
        // Act
        _target.DeleteTeam(1);
        // Assert
        A.CallTo(() => _teamService.GetTeamById(1)).MustHaveHappened();
        A.CallTo(() => _teamService.DeleteTeam(A<Team>._)).MustHaveHappened();
      }

      [Fact]
      public void GetTeams()
      {
        // Arrange
        
        // Act
        var result = _target.GetTeams(1);

        // Assert
        Check.That(result).IsInstanceOf<OkObjectResult>();
        A.CallTo(() => _teamService.GetTeams(1)).MustHaveHappened();
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
      public void RemovePlayer()
      {
        // Arrange
        
        // Act
        var result = _target.RemovePlayer(1, 1);
        // Assert
        A.CallTo(() => _teamService.RemovePlayer(1, 1)).MustHaveHappened();
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
