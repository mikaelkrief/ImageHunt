using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using Microsoft.EntityFrameworkCore;

namespace ImageHunt.Services
{
  public class GameService : AbstractService, IGameService
  {
    public GameService(HuntContext context) : base(context)
    {
    }

    public Game CreateGame(int adminId, Game newGame)
    {
      var admin = Context.Admins.Single(a => a.Id == adminId);
      var games = admin.Games ?? new List<Game>();
      games.Add(newGame);
      admin.Games = games;
      Context.SaveChanges();
      return newGame;
    }

    public Game GetGameById(int gameId)
    {
      return Context.Games.Include(g => g.Nodes).Include(g => g.Teams).Single(g => g.Id == gameId);
    }

    public IEnumerable<Game> GetGamesForAdmin(int adminId)
    {
      return Context.Admins.Include(a => a.Games).ThenInclude(g => g.Teams).Single(a => a.Id == adminId).Games;
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
  }
}
