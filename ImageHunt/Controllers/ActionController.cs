using System;
using System.Linq;
using System.Net;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Action = ImageHunt.Model.Action;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  #if !DEBUG
  [Authorize]
  #endif
  public class ActionController : BaseController
  {
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;
    private readonly IImageService _imageService;
    private readonly IActionService _actionService;
    private readonly INodeService _nodeService;
    private readonly ITeamService _teamService;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService,
      IActionService actionService,
      INodeService nodeService,
      ITeamService teamService)
    {
      _gameService = gameService;
      _playerService = playerService;
      _imageService = imageService;
      _actionService = actionService;
      _nodeService = nodeService;
      _teamService = teamService;
    }
    [HttpPost("AddGameAction")]
    public IActionResult AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = Mapper.Map<GameAction>(gameActionRequest);

      gameAction.Team = _teamService.GetTeamById(gameActionRequest.TeamId);
      gameAction.Game = _gameService.GetGameById(gameActionRequest.GameId);
      //gameAction.Latitude = gameActionRequest.Latitude;
      //gameAction.Longitude = gameActionRequest.Longitude;
      //gameAction.Action = (Action) gameActionRequest.Action;
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
      return CreatedAtAction("AddGameAction", gameAction);
    }
    
    [HttpPut("Validate/{gameActionId}")]
    [Authorize]
    public IActionResult Validate(int gameActionId)
    {
      var validatorId = UserId;
      _actionService.Validate(gameActionId, validatorId);
      return Ok();
    }
    [HttpGet("GetValidatedGameActionForGame/{gameId}")]
    public IActionResult GetValidatedGameActionForGame(int gameId)
    {
      return Ok(_actionService.GetValidatedGameActionForGame(gameId));
    }
  }
}
