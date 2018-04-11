
using FakeItEasy;
using ImageHunt.Model;
using ImageHunt.Request;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class ActionControllerTest
    {
      private ActionController _target;
      private IGameService _gameService;
      private IPlayerService _playerService;

      public ActionControllerTest()
      {
        _gameService = A.Fake<IGameService>();
        _playerService = A.Fake<IPlayerService>();
        _target = new ActionController(_gameService, _playerService);
      }
      [Fact]
      public void AddGameAction_UploadPicture()
      {
        // Arrange
        var gameActionRequest = new GameActionRequest()
        {
          Action = 2,
          Latitude = 1.2,
          Longitude = 50.2,
          GameId = 2,
          Picture = new byte[50],
          PlayerId = 5

        };
        // Act
        var result = _target.AddGameAction(gameActionRequest);
        // Assert
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _playerService.GetPlayerById(gameActionRequest.PlayerId)).MustHaveHappened();
        A.CallTo(() => _gameService.GetGameById(gameActionRequest.GameId)).MustHaveHappened();
      }
    }

  public class ActionController : Microsoft.AspNetCore.Mvc.Controller
  {
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;

    public ActionController(IGameService gameService, IPlayerService playerService)
    {
      _gameService = gameService;
      _playerService = playerService;
    }

    public IActionResult AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = new GameAction();
      gameAction.Player = _playerService.GetPlayerById(gameActionRequest.PlayerId);
      gameAction.Game = _gameService.GetGameById(gameActionRequest.GameId);
       switch (gameActionRequest.Action)
        {
          case 2:
            gameAction.Action = Action.SubmitPicture;
            break;
        }

      return Ok();
    }
  }
}