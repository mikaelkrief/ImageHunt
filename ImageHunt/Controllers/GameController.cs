using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class GameController : Controller
  {
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
      _gameService = gameService;
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
  }
}
