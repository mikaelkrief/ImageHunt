using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Action = ImageHuntWebServiceClient.Action;

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
    private readonly IHubContext<LocationHub> _hubContext;
    private readonly ILogger<ActionController> _logger;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService,
      IActionService actionService,
      INodeService nodeService,
      ITeamService teamService,
      IHubContext<LocationHub> hubContext,
      ILogger<ActionController> logger)
    {
      _gameService = gameService;
      _playerService = playerService;
      _imageService = imageService;
      _actionService = actionService;
      _nodeService = nodeService;
      _teamService = teamService;
      _hubContext = hubContext;
      _logger = logger;
    }
    /// <summary>
    /// Ad a game action from players to a game
    /// </summary>
    /// <param name="gameActionRequest"></param>
    [HttpPost("AddGameAction")]
    public async Task<IActionResult> AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = Mapper.Map<GameAction>(gameActionRequest);

      gameAction.Team = _teamService.GetTeamById(gameActionRequest.TeamId);
      gameAction.Game = _gameService.GetGameById(gameActionRequest.GameId);
      gameAction.Latitude = gameActionRequest.Latitude;
      gameAction.Longitude = gameActionRequest.Longitude;
      gameAction.Action = (Action)gameActionRequest.Action;
      if (gameActionRequest.NodeId != 0)
      {
        gameAction.Node = _nodeService.GetNode(gameActionRequest.NodeId);
      }
      gameAction.DateOccured = DateTime.Now;
      
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
      await NotifyClientsForGameAction(gameAction);

      return CreatedAtAction("AddGameAction", gameAction);
    }

    private async Task NotifyClientsForGameAction(GameAction gameAction)
    {
      await _hubContext.Clients.All.SendAsync("ActionSubmitted", gameAction);
    }

    [HttpPut("Validate/{gameActionId}")]
    [Authorize]
    public IActionResult Validate(int gameActionId)
    {
      var validatorId = UserId;
      _actionService.Validate(gameActionId, validatorId);
      return Ok();
    }
    [HttpPost("LogPosition")]
    public async Task<IActionResult> LogPosition(LogPositionRequest logPositionRequest)
    {
      _logger.LogInformation($"Received position gameId: {logPositionRequest.GameId}, teamId: {logPositionRequest.TeamId}, [{logPositionRequest.Latitude}, {logPositionRequest.Longitude}]");
      var gameAction = new GameAction()
        {
          Action = Action.SubmitPosition,
          Game = _gameService.GetGameById(logPositionRequest.GameId),
          Team = _teamService.GetTeamById(logPositionRequest.TeamId),
          Longitude = logPositionRequest.Longitude,
          Latitude = logPositionRequest.Latitude,
          DateOccured = DateTime.Now
        };
      _actionService.AddGameAction(gameAction);
      await NotifyClientsForGameAction(gameAction);
      return Ok();
    }
    [HttpGet("{gameId}")]
    public IActionResult GetGameActionsForGameAction(int gameId)
    {
      return Ok(_actionService.GetGamePositionsForGame(gameId));
    }
  }
}
