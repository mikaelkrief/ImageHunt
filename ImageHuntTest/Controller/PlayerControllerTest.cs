using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest
{
  [Collection("AutomapperFixture")]
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
