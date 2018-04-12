using System;
using ImageHunt.Model;
using ImageHunt.Request;
using ImageHunt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Action = ImageHunt.Model.Action;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class ActionController : Microsoft.AspNetCore.Mvc.Controller
  {
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;
    private readonly IImageService _imageService;
    private readonly IActionService _actionService;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService, IActionService actionService)
    {
      _gameService = gameService;
      _playerService = playerService;
      _imageService = imageService;
      _actionService = actionService;
    }
    [HttpPost("AddGameAction")]
    public IActionResult AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = new GameAction();
      gameAction.Player = _playerService.GetPlayerById(gameActionRequest.PlayerId);
      gameAction.Game = _gameService.GetGameById(gameActionRequest.GameId);
      switch (gameActionRequest.Action)
      {
        case 2:
          gameAction.Action = Action.SubmitPicture;
          var picture = new Picture();
          var bytes = Convert.FromBase64String(gameActionRequest.Picture);
          picture.Image = bytes;
          _imageService.AddPicture(picture);
          gameAction.Picture = picture;
          break;
      }
      _actionService.AddGameAction(gameAction);
      return Ok();
    }
  }
}
