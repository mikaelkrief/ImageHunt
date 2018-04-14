using System;
using System.Linq;
using ImageHunt.Model;
using ImageHunt.Model.Node;
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
    private readonly INodeService _nodeService;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService, IActionService actionService, INodeService nodeService)
    {
      _gameService = gameService;
      _playerService = playerService;
      _imageService = imageService;
      _actionService = actionService;
      _nodeService = nodeService;
    }
    [HttpPost("AddGameAction")]
    public IActionResult AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = new GameAction();
      gameAction.Player = _playerService.GetPlayerById(gameActionRequest.PlayerId);
      gameAction.Game = _gameService.GetGameById(gameActionRequest.GameId);
      gameAction.Latitude = gameActionRequest.Latitude;
      gameAction.Longitude = gameActionRequest.Longitude;
      gameAction.Action = (Action) gameActionRequest.Action;
      gameAction.Node = _nodeService.GetNode(gameActionRequest.NodeId);
      switch (gameAction.Action)
      {
        case Action.DoAction:
        case Action.SubmitPicture:
          var picture = new Picture();
          var bytes = Convert.FromBase64String(gameActionRequest.Picture);
          picture.Image = bytes;
          _imageService.AddPicture(picture);
          gameAction.Picture = picture;
          break;
        case Action.ReplyQuestion:
          var answer = _nodeService.GetAnswer(gameActionRequest.AnswerId);
          gameAction.SelectedAnswer = answer;
          var correctAnswer = ((QuestionNode) gameAction.Node).Answers.Single(a => a.Correct);
          gameAction.CorrectAnswer = correctAnswer;
          break;
      }
      _actionService.AddGameAction(gameAction);
      return Ok();
    }
  }
}
