using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Computation;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Response;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using NodeResponse = ImageHunt.Response.NodeResponse;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
#if !DEBUG
  [Authorize]
#endif
  public class GameController : BaseController
  {
    private readonly IGameService _gameService;
    private readonly IImageService _imageService;
    private readonly INodeService _nodeService;
    private readonly IActionService _actionService;
    private readonly ILogger<GameController> _logger;
    private readonly IImageTransformation _imageTransformation;

    public GameController(IGameService gameService,
      IImageService imageService,
      INodeService nodeService,
      IActionService actionService,
      ILogger<GameController> logger,
      IImageTransformation imageTransformation)
    {
      _gameService = gameService;
      _imageService = imageService;
      _nodeService = nodeService;
      _actionService = actionService;
      _logger = logger;
      _imageTransformation = imageTransformation;
    }

    [HttpGet("ById/{gameId}")]
    public IActionResult GetGameById(int gameId)
    {
      var gameById = _gameService.GetGameById(gameId);
      return Ok(gameById);
    }
    [HttpGet("ByAdminId/{adminId}")]
    public IActionResult GetGames(int adminId)
    {
      return Ok(_gameService.GetGamesForAdmin(adminId));
    }
    [HttpPost("{adminId}")]
    public IActionResult CreateGame(int adminId, [FromBody] Game newGame)
    {
      return CreatedAtAction("CreateGame", _gameService.CreateGame(adminId, newGame));
    }
    [HttpPost("AddNode/{gameId}")]
    public IActionResult AddNode(int gameId, [FromBody] AddNodeRequest nodeRequest)
    {
      if (nodeRequest == null)
        return BadRequest("Node is missing");
      var node = Mapper.Map<Node>(nodeRequest);
      switch (nodeRequest.NodeType)
      {
        case "FirstNode":
          var firstNode = node as FirstNode;
          firstNode.Password = nodeRequest.Password;
          break;
        case "TimerNode":
          var timerNode = node as TimerNode;
          timerNode.Delay = nodeRequest.Duration;
          break;
        case "ObjectNode":
          var objectNode = node as ObjectNode;
          objectNode.Action = nodeRequest.Action;
          break;
        case "QuestionNode":
          var questionNode = node as QuestionNode;
          questionNode.Question = nodeRequest.Question;
          questionNode.Answers = nodeRequest.Answers.Select(a => new Answer() { Response = a.Response, Correct = a.Correct }).ToList();
          break;
      }
      _gameService.AddNode(gameId, node);
      return CreatedAtAction("AddNode", node);
    }
    [HttpPut("AddPictures/{gameId}")]
    [DisableRequestSizeLimit]
    public IActionResult AddImageNodes(int gameId, List<IFormFile> files)
    {
      foreach (var file in files)
      {
        using (var fileStream = file.OpenReadStream())
        {
          byte[] bytes = new byte[fileStream.Length];
          fileStream.Read(bytes, 0, (int)fileStream.Length);
          var picture = new Picture() { Image = bytes };
          //_imageService.AddPicture(picture);
          var coordinates = _imageService.ExtractLocationFromImage(picture);
          // Drop the images without coordinates
          if (double.IsNaN(coordinates.Item1) || double.IsNaN(coordinates.Item2))
          {
            _logger.LogWarning($"The image {file.Name} is not geotagged");
            return new BadRequestObjectResult(new { message = $"The image {file.FileName}", filename = file.FileName });
          }

          var node = new PictureNode
          {
            Image = picture,
            Latitude = coordinates.Item1,
            Longitude = coordinates.Item2
          };
          _gameService.AddNode(gameId, node);
        }
      }
      return Ok();
    }
    [HttpPut("CenterGameByNodes/{gameId}")]
    public void SetCenterOfGameByNodes(int gameId)
    {
      _gameService.SetCenterOfGameByNodes(gameId);
    }
    [HttpGet("NodesRelations/{gameId}")]
    public IActionResult GetNodesRelations(int gameId)
    {
      var nodes = _gameService.GetNodes(gameId);
      var resNodes = new List<NodeResponse>();
      foreach (var node in nodes)
      {
        foreach (var nodeChild in node.Children)
        {
          var resNode = new NodeResponse(node);
          resNode.ChildNodeId = nodeChild.Id;
          resNodes.Add(resNode);

        }
      }
      return Ok(resNodes);
    }

    [HttpPatch("UpdateZoom/{gameId}/{zoom}")]
    public IActionResult UpdateZoom(int gameId, int zoom)
    {
      _gameService.SetGameZoom(gameId, zoom);
      return Ok();
    }
    [HttpGet("GetGameFromPlayerUserName/{playerUserName}")]
    public IActionResult GetGameFromPlayerUserName(string playerUserName)
    {
      return Ok(_gameService.GetGameFromPlayerChatId(playerUserName));
    }
    [HttpGet("FromLocation")]
    public IActionResult GetGamesFromLocation(double lat, double lng)
    {
      return Ok(_gameService.GetGamesFromPosition(lat, lng));
    }
    [HttpGet("GetQuestionNodeOfGame/{gameId}")]
    public IActionResult GetQuestionNodeOfGame(int gameId)
    {
      var questionNodeOfGame = _gameService.GetQuestionNodeOfGame(gameId);
      var questionNodesResponse = questionNodeOfGame.Select(n => new QuestionNodeResponse(n));
      return Ok(questionNodesResponse);
    }
    [HttpDelete("{gameId}")]
    public IActionResult DeleteGame(int gameId)
    {
      _gameService.DeleteGame(gameId);
      return Ok();
    }
    [HttpGet("GameActions")]
    public async Task<IActionResult> GetGameActions(GameActionListRequest gameActionListRequest)
    {
      var gameActions = await _actionService.GetGameActionsForGame(gameActionListRequest.GameId, gameActionListRequest.PageIndex, gameActionListRequest.PageSize);
      //foreach (var gameAction in gameActions)
      //{
      //  if (gameAction.Picture != null)
      //    gameAction.Picture.Image = _imageTransformation.Thumbnail(gameAction.Picture.Image, 150, 150);
      //}
      return Ok(gameActions);
    }
    [HttpPost("UploadImage")]
    public IActionResult UploadImage(IFormFile file)
    {
      if (file == null)
        return BadRequest("Image is bad format or null");
      using (var stream = file.OpenReadStream())
      {
        var image = new byte[stream.Length];
        stream.Read(image, 0, (int)stream.Length);
        var picture = new Picture() { Image = image };
        var coordinates = _imageService.ExtractLocationFromImage(picture);
        if (double.IsNaN(coordinates.Item1) || double.IsNaN(coordinates.Item2))
          return BadRequest();
        _imageService.AddPicture(picture);
        return CreatedAtAction("UploadImage", image);
      }
    }
    [HttpGet("GetGameAction/{gameActionId}")]
    public IActionResult GetGameAction(int gameActionId)
    {
      return Ok(_actionService.GetGameAction(gameActionId));
    }
    [HttpGet("GetImages/{gameId}")]
    public IActionResult GetImagesForGame(int gameId)
    {
      try
      {
        return Ok(_gameService.GetPictureNode(gameId).Select(p => p.Image));
      }
      catch (System.Exception e)
      {
        return BadRequest($"The {gameId} is not in the system or there are no images associated");
      }
    }
    [HttpGet("GameActionCount/{gameId}")]
    public IActionResult GetGameActionCountForGame(int gameId)
    {
      return Ok(_actionService.GetGameActionCountForGame(gameId));
    }
    [HttpGet("Reviewed")]
    public IActionResult GetGamesReviewed()
    {
      return Ok(_gameService.GetGamesWithScore());
    }
    /// <summary>
    /// Returns score of a game
    /// </summary>
    /// <param name="gameId">The gameId to get score</param>
    /// <returns>SScores ordered with teams and team's members</returns>
    [HttpGet("Score/{gameId}")]
    public IActionResult GetScoreForGame(int gameId)
    {
      return Ok(_actionService.GetScoresForGame(gameId));
    }
    [HttpGet("GetPictureNode/{gameId}")]
    public IActionResult GetPictureNodes(int gameId)
    {
      var picturesNodes = _gameService.GetPictureNode(gameId);
      foreach (var picturesNode in picturesNodes)
      {
        picturesNode.Image.Image = null;
      }

      return Ok(picturesNodes);
    }
    [HttpGet("GameActionsToValidate/{gameId}&pageIndex={pageIndex}&pageSize={pageSize}&nbPotential={nbPotential}")]
    public async Task<IActionResult> GetGameActionsToValidate(int gameId, int pageIndex, int pageSize, int nbPotential)
    {
      var gameActions = await _actionService.GetGameActionsForGame(gameId, pageIndex, pageSize);
      var gameActionsToValidate = new List<GameActionToValidate>();
      foreach (var gameAction in gameActions)
      {
        var gameActionToValidate = Mapper.Map<GameAction, GameActionToValidate>(gameAction);
        gameActionToValidate.ProbableNodes = _nodeService
          .GetGameNodesOrderByPosition(gameId, gameAction.Latitude, gameAction.Longitude).Take(nbPotential);
        foreach (var probableNode in gameActionToValidate.ProbableNodes)
        {
          if (probableNode is PictureNode)
            ((PictureNode)probableNode).Image = _imageService.GetImageForNode(probableNode);
        }

        gameActionToValidate.Node = gameActionToValidate.ProbableNodes.FirstOrDefault();
        gameActionsToValidate.Add(gameActionToValidate);
      }
      return Ok(gameActionsToValidate);
    }
  }
}
