using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    private readonly IImageService _imageService;

    public TeamController(ITeamService teamService,
                          IPlayerService playerService,
                          IImageService imageService)
    {
      _teamService = teamService;
      _playerService = playerService;
      _imageService = imageService;
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
    [HttpPut("StartTeam/{gameId}/{teamId}")]
    public IActionResult StartTeam(int gameId, int teamId)
    {
      var nextNode = _teamService.StartGame(gameId, teamId);
      return Ok(nextNode);
    }

    public IActionResult NextNodeForPlayer(int teamId)
    {
      return Ok(_teamService.NextNodeForTeam(teamId, 0, 0));
    }

    [HttpPost("UploadImage/")]
    [Consumes("multipart/form-data")]
    public IActionResult UploadImage(UploadImageRequest uploadRequest)
    {
      if (uploadRequest.FormFile == null)
        return BadRequest("Image is bad format or null");
      using (var stream = uploadRequest.FormFile.OpenReadStream())
      {
        var image = new byte[stream.Length];
        stream.Read(image, 0, (int) stream.Length);
        _teamService.UploadImage(uploadRequest.GameId, uploadRequest.TeamId, uploadRequest.Latitude, uploadRequest.Longitude, image);
        return Ok();
     }
   }

  }
}
