using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
#if !DEBUG
    [Authorize]
    #endif
  public class PlayerController : Controller
  {
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
      _playerService = playerService;
    }

    [HttpPost("CreatePlayer/{name}/{chatLogin}")]
    public IActionResult CreatePlayer(string name, string chatLogin)
    {
      return Ok(_playerService.CreatePlayer(name, chatLogin));
    }

    [HttpPut("JoinTeam/{teamId}/{playerId}")]
    public IActionResult JoinTeam(int teamId, int playerId)
    {
      return Ok(_playerService.JoinTeam(teamId, playerId));
    }

    [HttpGet("PlayerByChatId/{chatId}")]
    public IActionResult PlayerByChatId(string chatId)
    {
      return Ok(_playerService.GetPlayerByChatId(chatId));
    }
  }
}
