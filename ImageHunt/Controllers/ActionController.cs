using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,GameMaster,Validator,Bot")]
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
    private readonly IConfiguration _configuration;

    public ActionController(IGameService gameService,
      IPlayerService playerService,
      IImageService imageService,
      IActionService actionService,
      INodeService nodeService,
      ITeamService teamService,
      IHubContext<LocationHub> hubContext,
      IMapper mapper,
      ILogger<ActionController> logger,
      IConfiguration configuration,
      UserManager<Identity> userManager)
    :base(userManager)
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
      _configuration = configuration;
    }
    /// <summary>
    /// Add a game action from players to a game
    /// </summary>
    /// <param name="gameActionRequest"></param>
    [HttpPost("AddGameAction")]
    public async Task<IActionResult> AddGameAction(GameActionRequest gameActionRequest)
    {
      var gameAction = _mapper.Map<GameAction>(gameActionRequest);

      gameAction.Team = new Team() {Id = gameActionRequest.TeamId};
      gameAction.Game = new Game {Id = gameActionRequest.GameId};
      gameAction.Latitude = gameActionRequest.Latitude;
      gameAction.Longitude = gameActionRequest.Longitude;
      gameAction.Action = (Action)gameActionRequest.Action;
      gameAction.IsValidated = gameActionRequest.Validated;
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
        case Action.BonusNode:
          _teamService.SetBonus(gameAction.Team.Id, gameActionRequest.PointsEarned);
          break;
        case Action.DoAction:
        case Action.SubmitPicture:
          if (gameActionRequest.PictureId != 0)
          {
            gameAction.Picture = new Picture(){Id= gameActionRequest.PictureId};
          }

          if (!string.IsNullOrEmpty(gameActionRequest.Picture))
          {
            var picture = new Picture();
            var bytes = Convert.FromBase64String(gameActionRequest.Picture);
            picture.Image = bytes;
            await _imageService.AddPicture(picture);
            gameAction.Picture = picture;
          }

          break;
        case Action.ReplyQuestion:
          switch (gameAction.Node.NodeType)
          {
            case NodeResponse.QuestionNodeType:
              gameAction.Answer = gameActionRequest.Answer;
              break;
            case NodeResponse.ChoiceNodeType:
              var answer = _nodeService.GetAnswer(gameActionRequest.AnswerId);
              gameAction.SelectedAnswer = answer;
              if (gameAction.Node != null)
              {
                var correctAnswer = ((ChoiceNode) gameAction.Node).Answers.Single(a => a.Correct);
                gameAction.CorrectAnswer = correctAnswer;
              }
              break;
          }

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

    [HttpPut("Validate/{gameActionId}/{nodeId}")]
    public IActionResult Validate(int gameActionId, int nodeId)
    {
      //var validatorId = UserId;
      var validatorId = UserId;
      var gameAction = _actionService.Validate(gameActionId, nodeId, validatorId, true);
      return Ok(gameAction);
    }
    [HttpPut("Reject/{gameActionId}")]
    public IActionResult Reject(int gameActionId)
    {
      var validatorId = UserId;
      _actionService.Validate(gameActionId, 0, validatorId, false);
      return Ok();
    }

    [HttpPost("LogPosition")]
    public async Task<IActionResult> LogPosition(LogPositionRequest logPositionRequest)
    {
      _logger.LogInformation("Received position gameId: {0}, teamId: {1}, [{2}, {3}]",
        logPositionRequest.GameId,
        logPositionRequest.TeamId,
        logPositionRequest.Latitude,
        logPositionRequest.Longitude);
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
    public IActionResult GetGameActionsForGame(int gameId)
    {
      return Ok(_actionService.GetGamePositionsForGame(gameId));
    }
    [HttpGet("GetGameAction/{gameActionId}")]
    public IActionResult GetGameAction(int gameActionId)
    {
      var gameAction = _actionService.GetGameAction(gameActionId);
      var gameActionToValidate = _mapper.Map<GameActionToValidate>(gameAction);
      gameActionToValidate.ProbableNodes = _nodeService
        .GetGameNodesOrderByPosition(gameAction.Game.Id,
          gameAction.Latitude.Value, gameAction.Longitude.Value,
          NodeTypes.Picture | NodeTypes.Hidden | NodeTypes.Question)
        .Take(Convert.ToInt32(_configuration["NodeSettings:ProbableNodeNumber"]));
      foreach (var probableNode in gameActionToValidate.ProbableNodes)
      {
        if (probableNode is PictureNode)
          ((PictureNode)probableNode).Image = _imageService.GetImageForNode(probableNode);
      }

      return Ok(gameActionToValidate);
    }
    [HttpGet("GameActionCount")]
    public IActionResult GetGameActionCountForGame([FromQuery]GameActionCountRequest getGameActionCountRequest)
    {
      IncludeAction includeAction;
      Enum.TryParse(getGameActionCountRequest.IncludeAction, out includeAction);
      return Ok(_actionService.GetGameActionCountForGame(getGameActionCountRequest.GameId, includeAction, getGameActionCountRequest.TeamId));
    }
    [HttpGet("GameActionsToValidate")]
    public async Task<IActionResult> GetGameActionsToValidate([FromQuery]GameActionListRequest gameActionListRequest)
    {
      IncludeAction includeAction;
      Enum.TryParse(gameActionListRequest.IncludeAction, out includeAction);

      var gameActions = await _actionService.GetGameActionsForGame(gameActionListRequest.GameId,
        gameActionListRequest.PageIndex, gameActionListRequest.PageSize, includeAction, gameActionListRequest.TeamId);
      var gameActionsToValidate = new List<GameActionToValidate>();
      foreach (var gameAction in gameActions)
      {
        // Strip results when tea is deleted
        if (gameAction.Team == null)
          continue;

        var gameActionToValidate = _mapper.Map<GameAction, GameActionToValidate>(gameAction);
        FillProbableNodes(gameActionListRequest.GameId,
          gameActionListRequest.NbPotential, gameActionToValidate);
        gameActionToValidate.Node = gameActionToValidate.ProbableNodes.FirstOrDefault();
        gameActionsToValidate.Add(gameActionToValidate);

      }
      return Ok(gameActionsToValidate);
    }

    private void FillProbableNodes(int gameId, int nbPotential,
      GameActionToValidate gameActionToValidate)
    {
      if (gameActionToValidate.Latitude.HasValue && gameActionToValidate.Longitude.HasValue)
      {
        gameActionToValidate.ProbableNodes = _nodeService
          .GetGameNodesOrderByPosition(gameId,
            gameActionToValidate.Latitude.Value, gameActionToValidate.Longitude.Value,
            NodeTypes.Picture | NodeTypes.Hidden | NodeTypes.Question)
          .Take(nbPotential);
        foreach (var probableNode in gameActionToValidate.ProbableNodes)
        {
          if (probableNode is PictureNode)
            ((PictureNode) probableNode).Image = _imageService.GetImageForNode(probableNode);
        }

      }
    }

    [HttpGet("GameActions")]
    public async Task<IActionResult> GetGameActions([FromQuery]GameActionListRequest gameActionListRequest)
    {
      IncludeAction includeAction;
      Enum.TryParse(gameActionListRequest.IncludeAction, out includeAction);

      var gameActions = await _actionService.GetGameActionsForGame(gameActionListRequest.GameId,
        gameActionListRequest.PageIndex, gameActionListRequest.PageSize, includeAction, gameActionListRequest.TeamId);
      //foreach (var gameAction in gameActions)
      //{
      //  if (gameAction.Picture != null)
      //    gameAction.Picture.Image = _imageTransformation.Thumbnail(gameAction.Picture.Image, 150, 150);
      //}
      return Ok(gameActions);
    }
    [HttpPatch]
    public IActionResult Modify([FromBody]GameActionModifyRequest gameActionRequest)
    {
      if (gameActionRequest == null || gameActionRequest.Id == 0)
        return BadRequest();
      var gameAction = _actionService.GetGameAction(gameActionRequest.Id);
      gameAction.IsValidated = gameActionRequest.Validated;
      gameAction.IsReviewed = gameActionRequest.Reviewed;
      gameAction.PointsEarned = gameActionRequest.PointsEarned;
      _actionService.Update(gameAction);
      return Ok(_mapper.Map<GameActionResponse>(gameAction));
    }
    [HttpGet("Next/{gameId}/{gameActionId}")]
    public IActionResult GetNextGameAction(int gameId, int gameActionId)
    {
      var gameActionToValidate = _mapper.Map<GameActionToValidate>(_actionService.GetNextGameAction(gameId, gameActionId));
      FillProbableNodes(gameId, 5, gameActionToValidate);
      return Ok(gameActionToValidate);
    }
  }
}
