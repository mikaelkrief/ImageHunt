using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Action = ImageHuntCore.Model.Action;

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
    private readonly IGameService _gameService;
    private readonly IActionService _actionService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService,
      IPlayerService playerService,
      IImageService imageService,
      IGameService gameService,
      IActionService actionService,
      IMapper mapper)
    {
      _teamService = teamService;
      _playerService = playerService;
      _imageService = imageService;
      _gameService = gameService;
      _actionService = actionService;
      _mapper = mapper;
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
    public IActionResult AddPlayer(int teamId, [FromBody]PlayerRequest playerRequest)
    {
      var team = _teamService.GetTeamById(teamId);
      var player = _mapper.Map<Player>(playerRequest);
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

      return Ok(_teamService.GetTeamById(teamId));
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
    [HttpGet("GetTeamsOfPlayer/{gameId}/{playerChatId}")]
    public IActionResult GetTeamsOfPlayer(int gameId, string playerChatId)
    {
      var player = _playerService.GetPlayerByChatId(playerChatId);
      //var teams = _teamService.GetTeamsForPlayer(player);
      var game = _gameService.GetGameById(gameId);
      var teamPlayer = player.TeamPlayers.Single(t => game.Teams.Any(gt => gt == t.Team));
      var teamResponse = new TeamResponse()
      {
        GameId = game.Id,
        Id = teamPlayer.Team.Id,
        Name = teamPlayer.Team.Name
      };
      return Ok(teamResponse);
    }
    [HttpPut("StartTeam/{gameId}/{teamId}")]
    public IActionResult StartTeam(int gameId, int teamId)
    {
      var nextNode = _mapper.Map<NodeResponse>( _teamService.StartGame(gameId, teamId));
      return Ok(nextNode);
    }
    [HttpGet("NextNodeForTeam/{teamId}")]
    public IActionResult NextNodeForPlayer(int teamId)
    {
      return Ok(_teamService.NextNodeForTeam(teamId, 0, 0));
    }

    [HttpPost("UploadImage/")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(UploadImageRequest uploadRequest)
    {
      if (uploadRequest.FormFile != null)
      {
        using (var stream = uploadRequest.FormFile.OpenReadStream())
        {
          var image = new byte[stream.Length];
          stream.Read(image, 0, (int)stream.Length);
          var picture = new Picture(){Image = image};
          var location = _imageService.ExtractLocationFromImage(picture);
          if (!double.IsNaN(location.Item1) || !double.IsNaN(location.Item2))
          {
            uploadRequest.Latitude = location.Item1;
            uploadRequest.Longitude = location.Item2;
          }
          stream.Read(image, 0, (int) stream.Length);
          _teamService.UploadImage(uploadRequest.GameId, uploadRequest.TeamId, uploadRequest.Latitude, uploadRequest.Longitude, image, uploadRequest.ImageName);
        }

        return Ok();
      }

      if (uploadRequest.PictureId.HasValue)
      {
        var game = _gameService.GetGameById(uploadRequest.GameId);
        var team = _teamService.GetTeamById(uploadRequest.TeamId);
        var picture = await _imageService.GetPictureById(uploadRequest.PictureId.Value);
        var gameAction = new GameAction()
        {
          Action = Action.SubmitPicture,
          DateOccured = DateTime.Now,
          Game = game,
          Team = team,
          Picture = picture,
          Latitude = uploadRequest.Latitude,
          Longitude = uploadRequest.Longitude,
        };
        _actionService.AddGameAction(gameAction);
        return Ok();
      }

      return BadRequest();
   }
    [HttpDelete("RemoveByChatId/{teamId}/{chatId}")]
    public IActionResult RemovePlayer(int teamId, string chatId)
    {
      var team = _teamService.GetTeamById(teamId);
      var player = _playerService.GetPlayerByChatId(chatId);
      _teamService.DelMemberToTeam(team, player);
      return Ok();
    }
  }
}
