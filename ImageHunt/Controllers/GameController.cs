using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class GameController : Controller
  {
    private readonly IGameService _gameService;
    private readonly IImageService _imageService;

    public GameController(IGameService gameService, IImageService imageService)
    {
      _gameService = gameService;
      _imageService = imageService;
    }

    [HttpGet("ById/{gameId}")]
    public IActionResult GetGameById(int gameId)
    {
      return Ok(_gameService.GetGameById(gameId));
    }
    [HttpGet("ByAdminId/{adminId}")]
    public IActionResult GetGames(int adminId)
    {
      return Ok(_gameService.GetGamesForAdmin(adminId));
    }
    [HttpPost("{adminId}")]
    public IActionResult CreateGame(int adminId, [FromBody] Game newGame)
    {
      return Ok(_gameService.CreateGame(adminId, newGame));
    }
    [HttpPut("{gameId}")]
    public IActionResult AddNode(int gameId, [FromBody] Node node)
    {
      _gameService.AddNode(gameId, node);
      return Ok();
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
    [HttpPost("CenterGameByNodes/{gameId}")]
    public void SetCenterOfGameByNodes(int gameId)
    {
      _gameService.SetCenterOfGameByNodes(gameId);
    }
  }
}
