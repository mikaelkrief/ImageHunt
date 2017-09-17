using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  public class TeamController : Controller
  {
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
      _teamService = teamService;
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
      return Ok(team);
    }

    // PUT api/team/5
    [HttpPut("{teamId}")]
    public void AddPlayer(int teamId, [FromBody]Player player)
    {
      var team = _teamService.GetTeamById(teamId);
      _teamService.AddMemberToTeam(team, new List<Player>() { player });
    }

    // DELETE api/Team/5
    [HttpDelete("{id}")]
    public void DeleteTeam(int id)
    {
      var team = _teamService.GetTeamById(id);
      _teamService.DeleteTeam(team);
    }
  }
}
