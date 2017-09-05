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
        [HttpGet]
        public IActionResult GetTeams()
        {
            return Ok(_teamService.GetTeams());
        }

        // GET api/Team/5
        [HttpGet("{id}")]
        public IActionResult GetTeam(int id)
        {
            return Ok(_teamService.GetTeamById(id));
        }

        // POST api/Team
        [HttpPost]
        public IActionResult CreateTeam([FromBody]string teamName)
        {
          var team = new Team(){Name = teamName};
          _teamService.CreateTeam(team);
          return Ok(team);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
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
