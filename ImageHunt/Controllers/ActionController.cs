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
using ImageHuntWebServiceClient.Responses;
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
    private readonly IMapper _mapper;
    private readonly ILogger<ActionController> _logger;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService,
      IActionService actionService,
      INodeService nodeService,
      ITeamService teamService,
      IHubContext<LocationHub> hubContext,
      IMapper mapper,
      ILogger<ActionController> logger)
    {
      _gameService = gameService;
      _playerService = playerService;
      _imageService = imageService;
      _actionService = actionService;
      _nodeService = nodeService;
      _teamService = teamService;
      _hubContext = hubContext;
      _mapper = mapper;
      _logger = logger;
    }
    /// <summary>
    /// Ad a game action from players to a game
    /// </summary>
    /// <param name="gameActionRequest"></param>
    [HttpPost("AddGameAction")]
    public async Task<IActionResult> AddGameAction([FromBody]GameActionRequest gameActionRequest)
    {
      var gameAction = _mapper.Map<GameAction>(gameActionRequest);

      gameAction.Team = new Team() {Id = gameActionRequest.TeamId};
      gameAction.Game = new Game {Id = gameActionRequest.GameId};
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
        case Action.StartGame:
        case Action.EndGame:
          gameAction.IsValidated = true;
          break;
        case Action.GivePoints:
          gameAction.PointsEarned = gameActionRequest.PointsEarned;
          gameAction.IsValidated = true;
          break;
        case Action.DoAction:
        case Action.SubmitPicture:
          if (string.IsNullOrEmpty(gameActionRequest.Picture))
          {
            _logger.LogError("The picture Base64 string is empty, action is ignored");
            return BadRequest(gameActionRequest);
          }
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

      var response = _mapper.Map<GameAction, GameActionResponse>(gameAction);
      return CreatedAtAction("AddGameAction", response);
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
      _actionService.Validate(gameActionId, validatorId, true);
      return Ok();
    }
    [HttpPut("Reject/{gameActionId}")]
    [Authorize]
    public IActionResult Reject(int gameActionId)
    {
      var validatorId = UserId;
      _actionService.Validate(gameActionId, validatorId, false);
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
