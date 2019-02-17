using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Computation;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using SharpKml.Dom;
using SharpKml.Engine;
using ImageMagick;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


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
    private readonly IMapper _mapper;

    public GameController(IGameService gameService,
      IImageService imageService,
      INodeService nodeService,
      IActionService actionService,
      ILogger<GameController> logger,
      IImageTransformation imageTransformation,
      UserManager<Identity> userManager,
      IMapper mapper)
    : base(userManager)
    {
      _gameService = gameService;
      _imageService = imageService;
      _nodeService = nodeService;
      _actionService = actionService;
      _logger = logger;
      _imageTransformation = imageTransformation;
      _mapper = mapper;
    }

    [HttpGet("ById/{gameId}")]
    public IActionResult GetGameById(int gameId)
    {
      var gameById = _gameService.GetGameById(gameId);
      return Ok(gameById);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("ByUser")]
    public IActionResult GetGames()
    {
      var adminId = UserId;
      return Ok(_gameService.GetGamesForAdmin(adminId));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,GameMaster")]
    public async Task<IActionResult> CreateGame([FromBody] GameRequest newGame)
    {
      var adminId = UserId;
      var game = _mapper.Map<Game>(newGame);
      if (newGame.PictureId != 0)
        game.Picture = new Picture() {Id = newGame.PictureId};

      return CreatedAtAction("CreateGame", _gameService.CreateGame(adminId, game));
    }

    [HttpPost("AddNode/{gameId}")]
    public IActionResult AddNode(int gameId, [FromBody] AddNodeRequest nodeRequest)
    {
      if (nodeRequest == null)
        return BadRequest("Node is missing");
      var node = _mapper.Map<Node>(nodeRequest);
      switch (nodeRequest.NodeType)
      {
        case NodeResponse.FirstNodeType:
          var firstNode = node as FirstNode;
          firstNode.Password = nodeRequest.Password;
          break;
        case NodeResponse.TimerNodeType:
          var timerNode = node as TimerNode;
          timerNode.Delay = nodeRequest.Duration;
          break;
        case NodeResponse.ObjectNodeType:
          var objectNode = node as ObjectNode;
          objectNode.Action = nodeRequest.Action;
          break;
        case NodeResponse.HiddenNodeType:
          var hiddenNode = node as HiddenNode;
          hiddenNode.LocationHint = nodeRequest.Hint;
          break;
        case NodeResponse.BonusNodeType:
          var bonusNode = node as BonusNode;
          bonusNode.Location = nodeRequest.Location;
          bonusNode.BonusType = Enum.Parse<BonusNode.BONUS_TYPE>(nodeRequest.Bonus.ToString());
          break;
        case NodeResponse.ChoiceNodeType:
          var choiceNode = node as ChoiceNode;
          choiceNode.Choice = nodeRequest.Question;
          choiceNode.Answers = nodeRequest.Choices
            .Select(a => new Answer() {Response = a.Response, Correct = a.Correct}).ToList();
          break;
        case NodeResponse.QuestionNodeType:
          var questionNode = node as QuestionNode;
          questionNode.Question = nodeRequest.Question;
          questionNode.Answer = nodeRequest.Answer;
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
          fileStream.Read(bytes, 0, (int) fileStream.Length);
          var picture = new Picture() {Image = bytes};
          //_imageService.AddPicture(picture);
          var coordinates = _imageService.ExtractLocationFromImage(picture);
          fileStream.Seek(0, SeekOrigin.Begin);
          using (var magikImage = new MagickImage(fileStream))
          {
            magikImage.Quality = 80;
            magikImage.Strip();
            using (var compressedImageStream = new MemoryStream())
            {
              magikImage.Write(compressedImageStream);
              bytes = new byte[compressedImageStream.Length];
              compressedImageStream.Seek(0, SeekOrigin.Begin);
              compressedImageStream.Read(bytes, 0, (int)compressedImageStream.Length);
              picture = new Picture() { Image = bytes };
            }
          }
            // Drop the images without coordinates
            if (double.IsNaN(coordinates.Item1) || double.IsNaN(coordinates.Item2))
            {
              _logger.LogWarning($"The image {file.Name} is not geotagged");
              return new BadRequestObjectResult(new { message = $"The image {file.FileName}", filename = file.FileName });
            }

          var node = new PictureNode
          {
            Image = picture,
            Name = file.FileName,
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
      return Ok(_mapper.Map<IEnumerable<NodeResponse>>(nodes));
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
    public IActionResult GetChoiceNodeOfGame(int gameId)
    {
      var questionNodeOfGame = _gameService.GetChoiceNodeOfGame(gameId);
      var questionNodesResponse = _mapper.Map<IEnumerable<NodeResponse>>(questionNodeOfGame);
      return Ok(questionNodesResponse);
    }

    [HttpDelete("{gameId}")]
    public IActionResult DeleteGame(int gameId)
    {
      _gameService.DeleteGame(gameId);
      return Ok();
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
        var picture = new Picture() {Image = image};
        var coordinates = _imageService.ExtractLocationFromImage(picture);
        if (double.IsNaN(coordinates.Item1) || double.IsNaN(coordinates.Item2))
          return BadRequest();
        _imageService.AddPicture(picture);
        return CreatedAtAction("UploadImage", image);
      }
    }

    [HttpGet("GetImages/{gameId}")]
    public IActionResult GetImagesForGame(int gameId)
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<NodeResponse>>(_gameService.GetPictureNode(gameId).ToList()));
      }
      catch (System.Exception e)
      {
        return BadRequest($"The {gameId} is not in the system or there are no images associated");
      }
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
      var scores = _actionService.GetScoresForGame(gameId);

      return Ok(_mapper.Map<IEnumerable<ScoreResponse>>(scores));
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

    [HttpGet]
    public IActionResult GetAllGame()
    {
      return Ok(_mapper.Map<IEnumerable<GameResponse>>(_gameService.GetAllGame()));
    }

    [HttpGet("GameCode/{gameId}")]
    public IActionResult GetGameCode(int gameId)
    {
      return Ok(_gameService.GameCode(gameId));
    }

    [HttpPost("ImportKmlFile/{gameId}/{reverse}")]
    public IActionResult ImportKmlFile(int gameId, bool reverse, IFormFile file)
    {
      using (var stream = file.OpenReadStream())
      {
        var kmlFile = KmlFile.Load(stream) ;
        var kml = kmlFile.Root as Kml;
        foreach (var placemark in kml.Flatten().OfType<Placemark>())
        {
          var polygon = placemark.Geometry as Polygon;
          int index = 1;
          Node previousNode = null;
          var countCoordinates = polygon.OuterBoundary.LinearRing.Coordinates.Count;
          var coordinates = reverse? polygon.OuterBoundary.LinearRing.Coordinates.Reverse(): polygon.OuterBoundary.LinearRing.Coordinates;
          foreach (var coordinate in coordinates)
          {
             Node node;
           if (previousNode == null)
            {
              node = NodeFactory.CreateNode(NodeResponse.FirstNodeType);
            }
            else
            {
              node = NodeFactory.CreateNode(NodeResponse.WaypointNodeType);
            }
            if (index == countCoordinates - 1)
              node = NodeFactory.CreateNode(NodeResponse.LastNodeType);
            node.Latitude = coordinate.Latitude;
            node.Longitude = coordinate.Longitude;
            node.Name = $"Waypoint{index++}";
            _gameService.AddNode(gameId, node);
            if (previousNode != null)
              _nodeService.AddChildren(previousNode, node);
            previousNode = node;
          }
        }
      }

      return Ok();
    }

    [HttpGet("PathNodesCloseTo")]
    public IActionResult GetPathNodesCloseTo(NodeRequest nodeRequest)
    {
      var nodeType = Enum.Parse<NodeTypes>(nodeRequest.NodeType);
      var nodes = _nodeService.GetGameNodesOrderByPosition(nodeRequest.GameId, nodeRequest.Latitude, nodeRequest.Longitude,
        nodeType);
      return Ok(_mapper.Map<IEnumerable<NodeResponse>>(nodes));
    }
  }
}
