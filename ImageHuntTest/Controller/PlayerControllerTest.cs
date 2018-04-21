using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest
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
        var result = _target.JoinTeam(1, 1);
        // Assert
        Check.That(result).IsInstanceOf<OkObjectResult>();
        A.CallTo(() => _playerService.JoinTeam(1,1)).MustHaveHappened();
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
        A.CallTo(() => _playerService.NextNodeForPlayer(A<string>._, A<double>._, A<double>._)).MustHaveHappened();
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

        A.CallTo(() => _playerService.NextNodeForPlayer("playerName", A<double>._, A<double>._)).MustHaveHappened();
      }

      [Fact]
      public void UploadImage()
      {
        // Arrange
        
        // Act
        var result = _target.UploadImage("playerName", 15, 12, new byte[10]);
        // Assert
        Check.That(result).InheritsFrom<IActionResult>();
        A.CallTo(() => _playerService.UploadImage(A<string>._, A<double>._, A<double>._, A<byte[]>._))
          .MustHaveHappened();
      }

      [Fact]
      public void PlayerByChatId()
      {
        // Arrange
        
        // Act
        var result = _target.PlayerByChatId("toto");
        // Assert
        Check.That(result).IsInstanceOf<OkObjectResult>();
        A.CallTo(() => _playerService.GetPlayerByChatId("toto")).MustHaveHappened();
      }
  }

}
