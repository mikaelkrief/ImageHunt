using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageHunt.Data;
using ImageHunt.Helpers;
using ImageHuntCore.Computation;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
  public class GameService : AbstractService, IGameService
  {
    private readonly IMapper _mapper;
    public GameService(HuntContext context, ILogger<GameService> logger, IMapper mapper) : base(context, logger)
    {
      _mapper = mapper;
    }

    public Game CreateGame(int adminId, Game newGame)
    {
      var admin = Context.Admins.Single(a => a.Id == adminId);
      if (newGame.Picture != null && newGame.Picture.Id != 0)
        newGame.Picture = Context.Pictures.Single(p => p.Id == newGame.Picture.Id);
      string code;
      do
      {
        code = EntityHelper.CreateCode(6);
      } while (Context.Games.Any(g => g.Code == code));

      newGame.Code = code;
      Context.Games.Add(newGame);
      var gameAdmin = new GameAdmin() { Admin = admin, Game = newGame };

      admin.GameAdmins.Add(gameAdmin);
      Context.SaveChanges();
      return newGame;
    }

    public Game GetGameById(int gameId)
    {
      var game = Context.Games
        .Include(g => g.Nodes).ThenInclude(n=>n.Image)
        .Include(g=>g.Nodes).ThenInclude(n=>n.ChildrenRelation)
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
        .Single(a => a.Id == adminId).Games.Select(g =>
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
      if (node.Image != null)
      {
        var picture = Context.Pictures.SingleOrDefault(p => p.Id == node.Image.Id);
        node.Image = picture;
      }

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

    public IEnumerable<Node> GetNodes(int gameId, NodeTypes nodeType = NodeTypes.All)
    {
      IEnumerable<Node> nodes = Context.Games.Include(n => n.Nodes).ThenInclude(n => n.ChildrenRelation).Single(g => g.Id == gameId).Nodes;
      IEnumerable<Node> resNode = new List<Node>();
      if (nodeType.HasFlag(NodeTypes.All))
      {
        return nodes;
      }

      if (nodeType.HasFlag(NodeTypes.Picture))
      {
        resNode = resNode.Union(nodes.Where(n => n.NodeType == NodeResponse.PictureNodeType));
      }
      if (nodeType.HasFlag(NodeTypes.Hidden))
      {
        resNode = resNode.Union(nodes.Where(n => n.NodeType == NodeResponse.HiddenNodeType || n.NodeType == NodeResponse.BonusNodeType));
      }
      if (nodeType.HasFlag(NodeTypes.Path))
      {
        resNode = resNode.Union(nodes.Where(n => n.NodeType == NodeResponse.FirstNodeType ||
                                                 n.NodeType == NodeResponse.LastNodeType ||
                                                 n.NodeType == NodeResponse.ChoiceNodeType ||
                                                 n.NodeType == NodeResponse.ObjectNodeType ||
                                                 n.NodeType == NodeResponse.QuestionNodeType ||
                                                 n.NodeType == NodeResponse.TimerNodeType ||
                                                 n.NodeType == NodeResponse.WaypointNodeType));
      }
      return resNode;
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

    public IEnumerable<ChoiceNode> GetChoiceNodeOfGame(int gameId)
    {
      var game = Context.Games
        .Include(g => g.Nodes)
        .Single(g => g.Id == gameId);
      return Context.ChoiceNodes
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
      return Context.PictureNodes.Include(p => p.Image)
        .Where(p => pictureNodes.Any(pn => pn.Id == p.Id));
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

    public IEnumerable<Game> GetAllGame()
    {
      return Context.Games
        .Include(g=>g.Teams)
        .Where(g => g.IsActive && g.StartDate >= DateTime.Today && g.IsPublic);
    }

    public string GameCode(int gameId)
    {
      var game = Context.Games.Single(g => g.Id == gameId);
      string code;
      do
      {
        code = EntityHelper.CreateCode(6);
      } while (Context.Games.Any(g => g.Code == code));

      if (string.IsNullOrEmpty(game.Code))
      {
        game.Code = code;
      }

      return game.Code;
    }

    public Game Duplicate(Game orgGame, Admin admin)
    {
      Context.Attach(orgGame);
      Context.Attach(admin);
      // Copy the game

      string code;
      do
      {
        code = EntityHelper.CreateCode(6);
      } while (Context.Games.Any(g => g.Code == code));
      var newGame = new Game()
      {
        Name = $"{orgGame.Name}-2",
        Code = code,
        Description = orgGame.Description,
        IsActive = true,
        MapCenterLat = orgGame.MapCenterLat,
        MapCenterLng = orgGame.MapCenterLng,
        MapZoom = orgGame.MapZoom,
        NbPlayerPenaltyThreshold = orgGame.NbPlayerPenaltyThreshold,
        NbPlayerPenaltyValue = orgGame.NbPlayerPenaltyValue,
        Picture = orgGame.Picture,
        StartDate = DateTime.Today,
      };
      admin.GameAdmins.Add(new GameAdmin() { Game = newGame, Admin = admin });
      Context.Games.Add(newGame);
      Context.SaveChanges();
      return newGame;
    }

    public Game GetGameByCode(string gameCode)
    {
      return Context.Games
        .Include(g => g.Teams).ThenInclude(t => t.TeamPlayers)
        .Include(g => g.Picture)
        .Single(g => g.Code == gameCode);
    }

    public IEnumerable<Game> GetAllGameForValidation(Admin user)
    {
      Context.Attach(user);
      var gamesToValidate = user.Games.Where(g => g.IsActive && g.StartDate >= DateTime.Today);
      return gamesToValidate;
    }

    public Game Toogle(int gameId, Flag flagToChange = Flag.Active)
    {
      var game = Context.Games.Single(g => g.Id == gameId);
      switch (flagToChange)
      {
        case Flag.Public:
          game.IsPublic = !game.IsPublic;
          break;
        case Flag.Active:
          game.IsActive = !game.IsActive;
          break;
      }
      Context.SaveChanges();
      return game;
    }
  }
}
