using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Data;
using ImageHuntCore.Services;
using ImageHuntEngine;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntEngineTest
{
    public class PlayerServiceTest : ContextBasedTest
    {
      private PlayerService _target;

      public PlayerServiceTest()
      {
        _target = new PlayerService(_context);
      }
      [Fact]
      public void CreatePlayerTest()
      {
        // Arrange
        var players = new List<Player>()
        {
          new Player() {Name = "Titi", ChatLogin = "@titi"}
        };
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        _target.CreatePlayer("Toto", "chatLogin");
        // Assert
        Check.That(_context.Players).HasSize(2);
        Check.That(_context.Players.Any(p => p.Name == "Toto")).IsTrue();
      }

      [Fact]
      public void JoinTeam()
      {
        // Arrange
        var teams = new List<Team>(){new Team(){Name = "Team1"}, new Team(){Name = "Team2"}};
        _context.Teams.AddRange(teams);
        var games = new List<Game>() {new Game() {Name = "game1"}, new Game() {Name = "game2", Teams = new List<Team>(){ teams[1]}}};
        _context.Games.AddRange(games);
        var players = new List<Player>() {new Player() {Name = "Toto"}, new Player() {Name = "Titi"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        _target.JoinTeam("game2", "Team2", "Toto");
        // Assert
        Check.That(teams[1].Players).ContainsExactly(players[0]);
      }
      [Fact]
      public void JoinTeam_TeamDoesntExist()
      {
        // Arrange
        var teams = new List<Team>(){new Team(){Name = "Team1"}, new Team(){Name = "Team2"}};
        _context.Teams.AddRange(teams);
        var games = new List<Game>() {new Game() {Name = "game1"}, new Game() {Name = "game2", Teams = new List<Team>(){ teams[1]}}};
        _context.Games.AddRange(games);
        var players = new List<Player>() {new Player() {Name = "Toto"}, new Player() {Name = "Titi"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        Check.ThatCode(()=>_target.JoinTeam("game2", "Team3", "Toto")).Throws<ArgumentException>();
        // Assert
      }
      [Fact]
      public void JoinTeam_PlayerDoesntExist()
      {
        // Arrange
        var teams = new List<Team>(){new Team(){Name = "Team1"}, new Team(){Name = "Team2"}};
        _context.Teams.AddRange(teams);
        var games = new List<Game>() {new Game() {Name = "game1"}, new Game() {Name = "game2", Teams = new List<Team>(){ teams[1]}}};
        _context.Games.AddRange(games);
        var players = new List<Player>() {new Player() {Name = "Toto"}, new Player() {Name = "Titi"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        Check.ThatCode(()=> _target.JoinTeam("game2", "Team2", "Toto2")).Throws<ArgumentException>();
        // Assert
        Check.That(teams[1].Players).HasSize(0);
      }
      [Fact]
      public void JoinTeam_GameDoesntExist()
      {
        // Arrange
        var teams = new List<Team>(){new Team(){Name = "Team1"}, new Team(){Name = "Team2"}};
        _context.Teams.AddRange(teams);
        var games = new List<Game>() {new Game() {Name = "game1"}, new Game() {Name = "game2", Teams = new List<Team>(){ teams[1]}}};
        _context.Games.AddRange(games);
        var players = new List<Player>() {new Player() {Name = "Toto"}, new Player() {Name = "Titi"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        Check.ThatCode(()=> _target.JoinTeam("game3", "Team2", "Toto2")).Throws<ArgumentException>();
        // Assert
        Check.That(teams[1].Players).HasSize(0);
      }

      [Fact]
      public void StartPlayer_PlayerDoesntExist()
      {
        // Arrange
        var players = new List<Player>() {new Player() {Name = "Toto"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        Check.ThatCode(() => _target.StartPlayer("Titi")).Throws<ArgumentException>();
        // Assert
      }
      //[Fact]
      //public void StartPlayer_PlayerNotInTeam()
      //{
      //  // Arrange
      //  var players = new List<Player>() {new Player() {Name = "Toto"}};
      //  _context.Players.AddRange(players);
      //  var teams = new List<Team>() {new Team() { }};
      //  _context.SaveChanges();
      //  // Act
      //  Check.ThatCode(() => _target.StartPlayer("Toto")).Throws<ArgumentException>();
      //  // Assert
      //}
      [Fact]
      public void StartPlayer()
      {
        // Arrange
        var players = new List<Player>() {new Player() {Name = "Toto"}};
        _context.Players.AddRange(players);
        var teams = new List<Team>() {new Team() { Players = players}};
        _context.Teams.AddRange(teams);
        _context.SaveChanges();
        // Act
        _target.StartPlayer("Toto");
        // Assert
        Check.That(players[0].StartTime.Value.Date).Equals(DateTime.Today);
        //Check.That(_context.GameActions.First()).
      }

    }

  public class PlayerService : AbstractService, IPlayerService
  {

    public PlayerService(HuntContext context) : base(context)
    {
    }

    public Player CreatePlayer(string name, string chatLogin)
    {
      var player = new Player(){Name = name, ChatLogin = chatLogin};
      Context.Players.Add(player);
      Context.SaveChanges();
      return player;
    }

    public Player JoinTeam(string gameName, string teamName, string playerName)
    {
      var game = Context.Games.SingleOrDefault(g => g.Name == gameName);
      if (game == null)
        throw new ArgumentException($"Game {gameName} doesn't exist");
      var team = game.Teams.SingleOrDefault(t => t.Name == teamName);
      if (team == null)
        throw new ArgumentException($"Team {teamName} doesn't exist");
      var player = Context.Players.SingleOrDefault(p => p.Name == playerName);
      if (player == null)
        throw new ArgumentException($"Player {playerName} doesn't exist");
      if (team !=null && player != null)
      {
        team.Players.Add(player);
        Context.SaveChanges();
      }

      return player;
    }

    public void StartPlayer(string name)
    {
      var player = Context.Players.SingleOrDefault(p => p.Name == name);
      if (player == null)
        throw new ArgumentException($"Player {name} doesn't exist");
      //var team = Context.Teams.SingleOrDefault(t => t.Players.Contains(player));
      //if (team ==null)
      //  throw new ArgumentException($"Player {name} has no team");
      player.StartTime = DateTime.Now;
      Context.SaveChanges();
    }

    public Node NextNodeForPlayer(string playerName)
    {
      throw new NotImplementedException();
    }
  }
}
