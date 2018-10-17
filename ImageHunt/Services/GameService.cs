using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
  public class GameService : AbstractService, IGameService
  {
    public GameService(HuntContext context, ILogger<GameService> logger) : base(context, logger)
    {
    }

    public Game CreateGame(int adminId, Game newGame)
    {
      var admin = Context.Admins.Single(a => a.Id == adminId);
      if (newGame.Picture != null && newGame.Picture.Id != 0)
        newGame.Picture = Context.Pictures.Single(p => p.Id == newGame.Picture.Id);
      Context.Games.Add(newGame);
      var gameAdmin = new GameAdmin() {Admin = admin, Game = newGame};
      admin.GameAdmins.Add(gameAdmin);
      Context.SaveChanges();
      return newGame;
    }

    public Game GetGameById(int gameId)
    {
      var game = Context.Games
        .Include(g => g.Nodes)
        .Include(g => g.Teams)
        .Include(g => g.Picture)
        .Single(g => g.Id == gameId);
      if (game.StartDate.HasValue)
        game.StartDate = TimeZoneInfo.ConvertTimeToUtc(game.StartDate.Value, TimeZoneInfo.Utc);
      return game;
    }

    public IEnumerable<Game> GetGamesForAdmin(int adminId)
    {
      var gamesForAdmin = Context.Admins
        .Include(a => a.GameAdmins).ThenInclude(g => g.Game.Teams)
        .Include(a => a.GameAdmins).ThenInclude(g => g.Game.Picture)
        .Single(a => a.Id == adminId).Games.Select(g=>
        {
          if (g.StartDate.HasValue)
            g.StartDate = TimeZoneInfo.ConvertTimeToUtc(g.StartDate.Value, TimeZoneInfo.Utc);
          return g;
        });
      
      return gamesForAdmin;
    }

    public void AddNode(int gameId, Node node)
    {
      var game = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId);
      game.Nodes.Add(node);
      Context.SaveChanges();
    }

    public void SetCenterOfGameByNodes(int gameId)
    {
      var game = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId);
      var nodes = game.Nodes;
      var nodesPoints = nodes.Select(n => (n.Latitude, n.Longitude));
      var center = GeographyComputation.CenterOfGeoPoints(nodesPoints);
      game.MapCenterLat = center.Item1;
      game.MapCenterLng = center.Item2;
      Context.SaveChanges();
    }

    public IEnumerable<Node> GetNodes(int gameId)
    {
      var nodes = Context.Games.Include(n => n.Nodes).ThenInclude(n => n.ChildrenRelation).Single(g => g.Id == gameId).Nodes;
      return nodes;
    }

    public void SetGameZoom(int gameId, int zoom)
    {
      Context.Games.Single(g => g.Id == gameId).MapZoom = zoom;
      Context.SaveChanges();
    }

    public Game GetGameFromPlayerChatId(string playerChatUserName)
    {
      return Context.Games.Include(g => g.Teams).ThenInclude(t => t.TeamPlayers)
        .Include(g => g.Nodes)
        .Single(g => g.Teams.Any(t => t.Players.Any(p => p.ChatLogin == playerChatUserName)));
    }

    /// <summary>
    /// Returns all the games in a specific radius (5km)
    /// </summary>
    /// <param name="lat">latitude of the point to check games for</param>
    /// <param name="lng">longitude of the point to check games for</param>
    /// <returns>List of games where the center is less than 5km from the position</returns>
    public IEnumerable<Game> GetGamesFromPosition(double lat, double lng)
    {
      return Context.Games.Where(g => g.IsActive && g.MapCenterLat.HasValue && GeographyComputation.Distance(lat, lng, g.MapCenterLat.Value, g.MapCenterLng.Value) < 5000);
    }

    public IEnumerable<QuestionNode> GetQuestionNodeOfGame(int gameId)
    {
      var game = Context.Games
        .Include(g => g.Nodes)
        .Single(g => g.Id == gameId);
      return Context.QuestionNodes
        .Include(n => n.Answers)
        .Include(n => n.ChildrenRelation)
        .Where(n => game.Nodes.Contains(n));
    }

    public void DeleteGame(int gameId)
    {
      Context.Games.Remove(Context.Games.Single(g => g.Id == gameId));
      Context.SaveChanges();
    }

    public IEnumerable<PictureNode> GetPictureNode(int gameId)
    {
      var game = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId);
      var pictureNodes = game.Nodes.Where(n => n is PictureNode);
      return Context.PictureNodes.Include(p => p.Image).Where(p => pictureNodes.Any(pn => pn.Id == p.Id));
    }

    public IEnumerable<Game> GetGamesWithScore()
    {
      var games = Context.Games.Where(g => g.StartDate < DateTime.Now);
      var gamesActionWithScore =
        Context.GameActions.Include(g => g.Game)
          .Where(ga => ga.IsReviewed)
          .GroupBy(ga => ga.Game)
          .Select(ga => ga.Key);
      return games.Intersect(gamesActionWithScore);
    }

    public Game GetActiveGameForPlayer(Player player)
    {
      var games = Context.Games
        .Include(g => g.Teams).ThenInclude(t => t.TeamPlayers).ThenInclude(t => t.Player)
        .Where(g => g.IsActive && g.StartDate.HasValue && g.StartDate.Value.Date == DateTime.Today);
      var game = games.FirstOrDefault(g => g.Teams.Any(t => t.TeamPlayers.Any(p => p.Player == player)));
      return game;
    }
  }
}
