using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Exception;
using ImageHunt.Helpers;
using ImageHunt.Model;
using ImageHuntCore.Computation;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHunt.Services
{
  public class TeamService : AbstractService, ITeamService
  {
    public TeamService(HuntContext context, ILogger<TeamService> logger)
        : base(context, logger)
    {

    }

    public void CreateTeam(int gameId, Team team)
    {
      var game = Context.Games.Include(g => g.Teams).Single(g => g.Id == gameId);
      Picture picture = null;
      if (team.Picture != null)
      picture = Context.Pictures.SingleOrDefault(p=>p.Id == team.Picture.Id);
      team.Picture = picture;
      game.Teams.Add(team);
      string code;
      do
      {
        code = EntityHelper.CreateCode(6);
      } while (Context.Teams.Any(t => t.Code == code));

      team.Code = code;
      Context.SaveChanges();
    }

    public void DeleteTeam(Team team)
    {
      var teamToDelete = Context.Teams.Single(t => t.Id == team.Id);
      Context.Teams.Remove(teamToDelete);
      Context.SaveChanges();
    }

    public IEnumerable<Team> GetTeams(int gameId)
    {
      return Context.Games
        .Include(g => g.Teams).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
        .Include(g => g.Teams).ThenInclude(t => t.Picture)
        .Single(g => g.Id == gameId).Teams;
    }

    public void AddMemberToTeam(Team team, List<Player> players)
    {
      var teamToAddPlayers = Context.Teams.Single(t => t.Id == team.Id);
      teamToAddPlayers.TeamPlayers.AddRange(players.Select(p => new TeamPlayer() { Team = team, Player = p }));
      Context.SaveChanges();
    }

    public void DelMemberToTeam(Team team, Player playerToDelete)
    {
      var teamToModify = Context.Teams.Include(t => t.TeamPlayers).Single(t => t.Id == team.Id);
      var playerToRemove = Context.Players.Single(p => p.Id == playerToDelete.Id);
      teamToModify.TeamPlayers.Remove(teamToModify.TeamPlayers.Single(tp => tp.Player == playerToRemove));
      Context.SaveChanges();
    }

    public Team GetTeamByName(string teamName)
    {
      return Context.Teams.Include(t => t.TeamPlayers).ThenInclude(t => t.Player)

        .Single(t => t.Name == teamName);
    }

    public Team GetTeamById(int teamId)
    {
      return Context.Teams
        .Include(t => t.TeamPlayers).ThenInclude(t => t.Player)
        .Include(t => t.Picture)
        .Include(t => t.CurrentNode)
        .Single(t => t.Id == teamId);
    }



    public IEnumerable<Team> GetTeamsForPlayer(Player player)
    {
      var teamsWithPlayers = Context.Teams.Include(t => t.TeamPlayers).ThenInclude(t => t.Player);
      return teamsWithPlayers
        .Where(t => t.Players.Any(p => p.Id == player.Id));
    }
    public Node NextNodeForTeam(int teamId, double playerLatitude, double playerLongitude)
    {
      var team = GetTeamById(teamId);
      var currentGame = GetCurrentGameForTeam(team);
      if (currentGame == null || !currentGame.IsActive)
        throw new InvalidGameException();
      Node nextNode;
      if (team.CurrentNode == null)
        nextNode = currentGame.Nodes.Single(n => n.NodeType == "FirstNode");
      else
        nextNode = team.CurrentNode.Children.First();
      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Team = team,
        Game = currentGame,
        Longitude = playerLongitude,
        Latitude = playerLatitude,
        Node = team.CurrentNode
      };
      team.CurrentNode = nextNode;
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
      return nextNode;
    }

    public Team GetTeamForUserName(int gameId, string userName)
    {
      var game = Context.Games
        .Include(g => g.Teams).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
        .Single(g => g.Id == gameId);
      return game.Teams.SingleOrDefault(t => t.Players.Any(p => p.ChatLogin.Equals(userName, StringComparison.InvariantCultureIgnoreCase)));
    }

    public void UploadImage(int gameId, int teamId, double latitude, double longitude, byte[] image,
      string imageName = null)
    {
      if (image == null)
        throw new ArgumentException("Parameter image is not provided");
      var team = GetTeamById(teamId);
      var currentGame = Context.Games.Single(g => g.Id == gameId);
      var closestNode =
        Context.Nodes
          .OrderBy(n => GeographyComputation.Distance(n.Latitude, n.Longitude, latitude, longitude))
        .FirstOrDefault();

      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Game = currentGame,
        Team = team,
        Latitude = latitude,
        Longitude = longitude,
        Picture = new Picture() { Image = image },
        Action = Action.SubmitPicture,
        Node = closestNode
      };
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
    }

    public void SetBonus(int teamId, int bonusValue)
    {
      var team = Context.Teams.Single(t => t.Id == teamId);
      team.Bonus = bonusValue;
      Context.SaveChanges();
    }


    private Game GetCurrentGameForTeam(Team team)
    {
      var currentGame = Context.Games
        .Include(g => g.Teams)
        .Include(g => g.Nodes)
        .Single(g => g.Teams.Any(gt => gt == team));
      return currentGame;
    }

    public Node StartGame(int gameId, int teamId)
    {
      var team = GetTeamById(teamId);
      var game = GetCurrentGameForTeam(team);
      if (!game.IsActive)
        throw new ArgumentException("There is no game active");
      var currentNode = game.Nodes.FirstOrDefault(n => n is FirstNode);
      team.CurrentNode = Context.Nodes.Include(n => n.ChildrenRelation).ThenInclude(cr => cr.Children)
        .Single(n => n == currentNode);
      Context.SaveChanges();
      return team.CurrentNode;
    }

  }
}
