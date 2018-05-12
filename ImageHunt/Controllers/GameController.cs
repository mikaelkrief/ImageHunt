using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Response;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  #if !DEBUG
  [Authorize]
  #endif
  public class GameController : Controller
  {
    private readonly IGameService _gameService;
    private readonly IImageService _imageService;
    private readonly INodeService _nodeService;
    private readonly IActionService _actionService;

    public GameController(IGameService gameService,
      IImageService imageService,
      INodeService nodeService,
      IActionService actionService)
    {
      _gameService = gameService;
      _imageService = imageService;
      _nodeService = nodeService;
      _actionService = actionService;
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
          questionNode.Answers = nodeRequest.Answers.Select(a => new Answer() {Response = a.Response, Correct = a.Correct}).ToList();
          break;
     }
      _gameService.AddNode(gameId, node);
      return CreatedAtAction("AddNode", node);
    }
    [HttpPut("AddPictures/{gameId}")]
    public IActionResult AddImageNodes(int gameId, List<IFormFile> files)
    {
      foreach (var file in files)
      {
        using (var fileStream = file.OpenReadStream())
        {
          byte[] bytes = new byte[fileStream.Length];
          fileStream.Read(bytes, 0, (int)fileStream.Length);
          var picture = new Picture(){Image = bytes};
          //_imageService.AddPicture(picture);
          var coordinates = _imageService.ExtractLocationFromImage(picture);
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
    [HttpGet("GetGameActions/{gameId}")]
    public IActionResult GetGameActions(int gameId)
    {
      return Ok(_actionService.GetGameActionsForGame(gameId));
    }
    [HttpPost("UploadImage")]
    public IActionResult UploadImage(IFormFile file)
    {
      if (file == null)
        return BadRequest("Image is bad format or null");
      using (var stream = file.OpenReadStream())
      {
        var image = new byte[stream.Length];
        stream.Read(image, 0, (int) stream.Length);
        var picture = new Picture() {Image = image };
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
        return Ok(_gameService.GetPictureNode(gameId).Select(p=>p.Image));
      }
      catch (System.Exception e)
      {
        return BadRequest($"The {gameId} is not in the system or there are no images associated");
      }
    }
  }
}
