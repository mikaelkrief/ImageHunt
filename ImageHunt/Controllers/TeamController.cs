using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  #if !DEBUG
  [Authorize]
  #endif
  public class TeamController : Controller
  {
    private readonly ITeamService _teamService;
    private readonly IPlayerService _playerService;

    public TeamController(ITeamService teamService, IPlayerService playerService)
    {
      _teamService = teamService;
      _playerService = playerService;
    }
    // GET: api/Team
    [HttpGet("ByGame/{gameId}")]
    public IActionResult GetTeams(int gameId)
    {
      return Ok(_teamService.GetTeams(gameId));
    }

    // GET api/Team/5
    [HttpGet("{teamId}")]
    public IActionResult GetTeam(int teamId)
    {
      return Ok(_teamService.GetTeamById(teamId));
    }

    // POST api/Team
    [HttpPost("{gameId}")]
    public IActionResult CreateTeam(int gameId, [FromBody]Team team)
    {
      _teamService.CreateTeam(gameId, team);
      return CreatedAtAction("CreateTeam", team);
    }

    // PUT api/team/5
    [HttpPost("AddPlayer/{teamId}")]
    public void AddPlayer(int teamId, [FromBody]Player player)
    {
      var team = _teamService.GetTeamById(teamId);
      Player dbPlayer = null;
      try
      {
        dbPlayer = _playerService.GetPlayerByChatId(player.ChatLogin);
        _playerService.JoinTeam(teamId, dbPlayer.Id);
      }
      catch (InvalidOperationException )
      {
        _teamService.AddMemberToTeam(team, new List<Player>() { player });
      }
    }

    // DELETE api/Team/5
    [HttpDelete("{id}")]
    public void DeleteTeam(int id)
    {
      var team = _teamService.GetTeamById(id);
      _teamService.DeleteTeam(team);
    }
    [HttpGet("Player/{playerLogin}")]
    public IActionResult GetPlayer(string playerLogin)
    {
      return Ok(_playerService.GetPlayerByChatId(playerLogin));
    }
    [HttpDelete("Remove/{teamId}/{playerId}")]
    public IActionResult RemovePlayer(int teamId, int playerId)
    {
      var team = _teamService.GetTeamById(teamId);
      var player = _playerService.GetPlayerById(playerId);
      _teamService.DelMemberToTeam(team, player);
      return Ok();
    }
    [HttpGet("GetTeamsOfPlayer/{playerChatId}")]
    public IActionResult GetTeamsOfPlayer(string playerChatId)
    {
      var player = _playerService.GetPlayerByChatId(playerChatId);
      return Ok(_teamService.GetTeamsForPlayer(player));
    }
    [HttpPut("StartPlayer/{gameId}/{teamId}")]
    public IActionResult StartPlayer(int gameId, int teamId)
    {
      _teamService.StartGame(gameId, teamId);
      var nextNode = _teamService.NextNodeForTeam(teamId, 0, 0);
      return Ok(nextNode);
    }

    public IActionResult NextNodeForPlayer(int teamId)
    {
      return Ok(_teamService.NextNodeForTeam(teamId, 0, 0));
    }

    public IActionResult UploadImage(int teamId, int latitude, int longitude, byte[] image)
    {
      _teamService.UploadImage(teamId, latitude, longitude, image);
      return Ok();
    }

  }
}
