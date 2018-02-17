using System;
using FakeItEasy;
using ImageHunt.Model.Node;
using ImageHuntEngine;
using ImageHuntEngine.Controllers;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntEngineTest
{
    public class PlayerControllerTest
    {
      private IPlayerService _playerService;
      private PlayerController _target;

      public PlayerControllerTest()
      {
        _playerService = A.Fake<IPlayerService>();
        _target = new PlayerController(_playerService);
      }
        [Fact]
        public void RegisterPlayer()
        {
          // Arrange
          // Act
          var result = _target.CreatePlayer("Toto", "@Toto");
          // Assert
          Check.That(result).IsInstanceOf<OkObjectResult>();
          A.CallTo(() => _playerService.CreatePlayer(A<string>._, A<string>._)).MustHaveHappened();
        }

      [Fact]
      public void PlayerJoinTeam()
      {
        // Arrange
        
        // Act
        var result = _target.JoinTeam("game","Toto", "Team");
        // Assert
        Check.That(result).IsInstanceOf<OkObjectResult>();
        A.CallTo(() => _playerService.JoinTeam(A<string>._, A<string>._, A<string>._)).MustHaveHappened();
      }

      [Fact]
      public void StartPlayer()
      {
        // Arrange
        // Act
        var result = _target.StartPlayer("playerName");
        // Assert
        Check.That(result).IsInstanceOf<OkObjectResult>();
        var nextNode = ((OkObjectResult) result).Value;
        Check.That(nextNode).InheritsFrom<Node>();
        A.CallTo(() => _playerService.StartPlayer(A<string>._)).MustHaveHappened();
        A.CallTo(() => _playerService.NextNodeForPlayer(A<string>._)).MustHaveHappened();
      }

      [Fact]
      public void NextNodeForPlayer()
      {
        // Arrange
        
        // Act
        var result = _target.NextNodeForPlayer("playerName");
        // Assert
        Check.That(result).InheritsFrom<IActionResult>();
        var nextNode = ((OkObjectResult)result).Value;
        Check.That(nextNode).InheritsFrom<Node>();

        A.CallTo(() => _playerService.NextNodeForPlayer("playerName")).MustHaveHappened();
      }
  }

}
