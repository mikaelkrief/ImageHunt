using System;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
      private readonly IPlayerService _playerService;

      public PlayerController(IPlayerService playerService)
      {
        _playerService = playerService;
      }

      public IActionResult CreatePlayer(string name, string chatLogin)
      {
        return Ok(_playerService.CreatePlayer(name, chatLogin));
      }

      public IActionResult JoinTeam(string gameName, string name, string team)
      {
        return Ok(_playerService.JoinTeam(gameName, team, name ));
      }

      public IActionResult StartPlayer(string playername)
      {
        _playerService.StartPlayer(playername);
        var nextNode = _playerService.NextNodeForPlayer(playername, 0,0);
        return Ok(nextNode);
      }

      public IActionResult NextNodeForPlayer(string playername)
      {
        return Ok(_playerService.NextNodeForPlayer(playername,0,0));
      }

      public IActionResult UploadImage(string playername, int latitude, int longitude, byte[] image)
      {
        _playerService.UploadImage(playername, latitude, longitude, image);
        return Ok();
      }
    }
}
