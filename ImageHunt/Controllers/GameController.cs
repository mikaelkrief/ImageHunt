using System.Collections.Generic;
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
    }
}
