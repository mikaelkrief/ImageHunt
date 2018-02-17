using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ImageHuntEngine.Controllers
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
        var nextNode = _playerService.NextNodeForPlayer(playername);
        return Ok(nextNode);
      }

      public IActionResult NextNodeForPlayer(string playername)
      {
        return Ok(_playerService.NextNodeForPlayer(playername));
      }
    }
}
