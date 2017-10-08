using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.CodeAnalysis;
using NFluent;
using SQLitePCL;
using Xunit;

namespace ImageHuntTest.Services
{
  public class GameServiceTest : ContextBasedTest
  {
    private GameService _target;

    public GameServiceTest()
    {
      _target = new GameService(_context);
    }

    [Fact]
    public void CreateGame()
    {
      // Arrange
      var admins = new List<Admin>() { new Admin(), new Admin(), new Admin() };
      _context.Admins.AddRange(admins);
      _context.SaveChanges();
      var game = new Game();
      // Act
      var result = _target.CreateGame(admins[1].Id, game);
      // Assert
      Check.That(result.Id).Not.IsEqualTo(0);
      Check.That(admins[1].Games).ContainsExactly(game);
    }

    //[Fact]
    //public void CreateGameFirstNodeNotInNodes()
    //{
    //    // Arrange
    //    var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode(), new QuestionNode() };
    //    // Act
    //    Check.ThatCode(() => _target.CreateGame("TheGame", DateTime.Today, nodes)).Throws<ArgumentException>();
    //    // Assert
    //}

    [Fact]
    public void GetGameFromId()
    {
      // Arrange
      var games = new List<Game>()
            {
                new Game(),
                new Game(),
                new Game()
            };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var result = _target.GetGameById(2);
      // Assert
      Check.That(result).IsEqualTo(games[1]);
    }

    [Fact]
    public void GetGamesForAdmin()
    {
      // Arrange
      var games = new List<Game>()
            {
                new Game(),
                new Game()
            };
      var admin = new Admin() { Games = games };
      _context.Admins.Add(admin);
      _context.SaveChanges();
      // Act
      var results = _target.GetGamesForAdmin(admin.Id);
      // Assert
      Check.That(results).ContainsExactly(games);
    }

    [Fact]
    public void AddNode()
    {
      // Arrange
      var games = new List<Game>() { new Game(), new Game() { Nodes = new List<Node>() { new TimerNode() } }, new Game() };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      var node = new TimerNode();
      // Act
      _target.AddNode(games[1].Id, node);
      // Assert
      Check.That(games[1].Nodes).HasSize(2);
    }

    [Fact]
    public void GetNodes()
    {
      // Arrange
      var nodes = new List<Node>() { new TimerNode() { Name = "First" }, new TimerNode() { Name = "Second" }, new TimerNode() { Name = "Third" }, new TimerNode() { Name = "Fourth" } };
      var games = new List<Game>() { new Game(), new Game() { Nodes = nodes } };
      nodes[0].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[0], Children = nodes[1] });
      nodes[1].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[1], Children = nodes[2] });
      nodes[2].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[2], Children = nodes[3] });
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var resNodes = _target.GetNodes(games[1].Id);
      // Assert
      Check.That(resNodes).ContainsExactly(nodes);
      Check.That(nodes[0].Children).ContainsExactly(nodes[1]);
    }
    [Fact]
    public void SetCenterOfGameByNodes()
    {
      // Arrange
      var nodes = new List<Node>()
        {
          new PictureNode(){Latitude = 48.8501065, Longitude = 2.327722},
          new TimerNode(){Latitude = 48.851291, Longitude = 2.3318698},
          new QuestionNode(){Latitude = 48.8537828, Longitude = 2.3310879}
        };
      var games = new List<Game>() { new Game(), new Game() { Nodes = nodes } };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      _target.SetCenterOfGameByNodes(games[1].Id);
      // Assert
      Check.That(games[1].MapCenterLat.Value).IsEqualsWithDelta(48.8517267806692, 0.0001);
      Check.That(games[1].MapCenterLng.Value).IsEqualsWithDelta(2.33022653262665, 0.0001);
    }

    [Fact]
    public void SetGameZoom()
    {
      // Arrange
      var games = new List<Game>() { new Game(), new Game(), new Game() };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      _target.SetGameZoom(games[1].Id, 15);
      // Assert
      Check.That(games[1].MapZoom).Equals(15);
    }

    [Fact]
    public void AddGameAction()
    {
      // Arrange

      // Act

      // Assert
    }

    [Fact]
    public void GetGameFromChatId()
    {
      // Arrange
      var users = new List<Player>()
        {
          new Player() {ChatLogin = "toto"},
          new Player() {ChatLogin = "Titi"},
          new Player() {ChatLogin = "tata"}
        };
      var teams = new List<Team>() { new Team(), new Team() { Players = users } };
      var games = new List<Game>() { new Game(), new Game() { Teams = teams } };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var game = _target.GetGameFromPlayerChatId("Titi");
      // Assert
      Check.That(game).Equals(games[1]);
    }

    [Fact]
    public void GetGamesFromPosition()
    {
      // Arrange
      var games = new List<Game>()
      {
        new Game() {IsActive = true, Name = "Game1", MapCenterLat = 48.846624, MapCenterLng = 2.335406 },
        new Game() {IsActive = true, Name = "Game2", MapCenterLat = 48.851491, MapCenterLng = 2.325831 },
        new Game() {IsActive = true, Name = "Game3", MapCenterLat = 48.856271, MapCenterLng = 2.29314 },
        new Game() {IsActive = true, Name = "Game4", MapCenterLat = 48.830193, MapCenterLng = 2.252212 },
        new Game() {IsActive = true, Name = "Game5", MapCenterLat = 48.822723, MapCenterLng = 2.209079 },
      };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var result = _target.GetGamesFromPosition(48.846906, 2.33773);
      // Assert
      Check.That(result).Contains(games[0], games[1], games[2]);
    }
  }
}
