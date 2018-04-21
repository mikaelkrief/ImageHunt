using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using ImageHunt.Exception;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using Action = ImageHunt.Model.Action;

namespace ImageHuntTest
{
    public class PlayerServiceTest : ContextBasedTest
    {
      private PlayerService _target;
      private ILogger<PlayerService> _logger;

      public PlayerServiceTest()
      {
        _logger = A.Fake<ILogger<PlayerService>>();
        _target = new PlayerService(_context, _logger);
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
        BooleanCheckExtensions.IsTrue(Check.That(_context.Players.Any(p => p.Name == "Toto")));
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
        _target.JoinTeam(teams[1].Id, players[0].Id);
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
        Check.ThatCode(()=>_target.JoinTeam(-1, players[0].Id)).Throws<InvalidOperationException>();
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
        Check.ThatCode(()=> _target.JoinTeam(players[1].Id, -1)).Throws<InvalidOperationException>();
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
        var teams = new List<Team>() {new Team()};
        var teamPlayers = players.Select(p => new TeamPlayer() {Player = p, Team = teams[0]});
        teams[0].TeamPlayers = teamPlayers.ToList();
        var nodes = new List<Node>() {new FirstNode() {Latitude = 10, Longitude = 12}};
        var games = new List<Game>() {new Game() {Teams = teams, IsActive = true, Name = "Game1", StartDate = DateTime.Now, Nodes = nodes}};
        players[0].CurrentGame = games[0];
        _context.Nodes.AddRange(nodes);
        _context.Teams.AddRange(teams);
        _context.Games.AddRange(games);
        _context.SaveChanges();
        // Act
        _target.StartPlayer("Toto");
        // Assert
        Check.That(players[0].StartTime.Value.Date).Equals(DateTime.Today);
        Check.That(players[0].CurrentNode).Equals(nodes[0]);
      }
      [Fact]
      public void StartPlayer_GameNotActive()
      {
        // Arrange
        var players = new List<Player>() {new Player() {Name = "Toto"}};
        _context.Players.AddRange(players);
        var teams = new List<Team>() {new Team()};
        var teamPlayers = players.Select(p => new TeamPlayer() { Player = p, Team = teams[0] });
        teams[0].TeamPlayers = teamPlayers.ToList();
        var nodes = new List<Node>() {new FirstNode() {Latitude = 10, Longitude = 12}};
        var games = new List<Game>() {new Game() {Teams = teams, IsActive = false, Name = "Game1", StartDate = DateTime.Now, Nodes = nodes}};
        players[0].CurrentGame = games[0];
        _context.Nodes.AddRange(nodes);
        _context.Teams.AddRange(teams);
        _context.Games.AddRange(games);
        _context.SaveChanges();
        // Act
        Check.ThatCode(() => _target.StartPlayer("Toto")).Throws<ArgumentException>();
        // Assert
      }

      [Fact]
      public void NextNodeForPlayer()
      {
      // Arrange
        var nodes = new List<Node>() {new FirstNode(){Latitude = 10, Longitude = 11}, new ObjectNode(), new PictureNode()};
        var childrenRelations = new List<ParentChildren>()
        {
          new ParentChildren(){Parent = nodes[0], Children = nodes[1]}
        };
        _context.ParentChildren.AddRange(childrenRelations);
        nodes[0].ChildrenRelation = childrenRelations;
        _context.Nodes.AddRange(nodes);
        var games = new List<Game>()
        {
          new Game() {Nodes = nodes, IsActive = true}
        };
        _context.Games.AddRange(games);
        var players = new List<Player>()
        {
          new Player() { Name = "Toto", CurrentGame = games[0], CurrentNode = nodes[0]}
        };
        _context.Players.AddRange(players);

        _context.SaveChanges();
        // Act
        var result = _target.NextNodeForPlayer("Toto", 15, 16);
        // Assert
        Check.That(players[0].CurrentNode).Equals(nodes[1]);
        Check.That(result).Equals(nodes[1]);
        Check.That(_context.GameActions).HasSize(1);
        Check.That(_context.GameActions.Extracting("Game")).Contains(games[0]);
        Check.That(_context.GameActions.Extracting("Player")).Contains(players[0]);
        Check.That(_context.GameActions.Extracting("Latitude")).Contains(15);
        Check.That(_context.GameActions.Extracting("Longitude")).Contains(16);
        Check.That(_context.GameActions.Extracting("Node")).Contains(nodes[0]);
      }
      [Fact]
      public void NextNodeForPlayer_GameNotStarted()
      {
      // Arrange
        var nodes = new List<Node>() {new FirstNode(), new ObjectNode(), new PictureNode()};
        var childrenRelations = new List<ParentChildren>()
        {
          new ParentChildren(){Parent = nodes[0], Children = nodes[1]}
        };
        _context.ParentChildren.AddRange(childrenRelations);
        nodes[0].ChildrenRelation = childrenRelations;
        _context.Nodes.AddRange(nodes);
        var games = new List<Game>()
        {
          new Game() {Nodes = nodes}
        };
        _context.Games.AddRange(games);
        var players = new List<Player>()
        {
          new Player() { Name = "Toto", CurrentGame = games[0]}
        };
        _context.Players.AddRange(players);

        _context.SaveChanges();
        // Act
        Check.ThatCode(() => _target.NextNodeForPlayer("Toto", 0, 0)).Throws<InvalidGameException>();
        // Assert
      }

      [Fact]
      public void UploadImage_PlayerDoesntExist()
      {
        // Arrange
        
        // Act
        Check.ThatCode(() => _target.UploadImage("Toto", 10,10, null)).Throws<ArgumentException>();
        // Assert
      }

      [Fact]
      public void UploadImage_NoImageProvided()
      {
      // Arrange
        var players = new List<Player>() {new Player() {Name = "Toto"}};
        _context.Players.AddRange(players);
        _context.SaveChanges();
      // Act
      Check.ThatCode(() => _target.UploadImage("Toto", 10, 10, null)).Throws<ArgumentException>();
        // Assert
    }

      [Fact]
      public void UploadImage()
      {
      // Arrange
        var players = new List<Player>() { new Player() { Name = "Toto" } };
        var image1 = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
        var image2 = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.ingress_20180128_130017_1.jpg");
        var nodes = new List<Node>()
        {
          new PictureNode(){Image = new Picture(){Image = image1 } },
          new PictureNode(){Image = new Picture(){Image = image2}}
        };
        _context.Nodes.AddRange(nodes);
        var games = new List<Game>(){new Game(){Nodes = nodes}};
        players[0].CurrentGame = games[0];
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        _target.UploadImage("Toto", 59.3278160094444, 18.0551338194444, image1);
      // Assert
        var imageAction = _context.GameActions.First();
        Check.That(imageAction.Game).Equals(games[0]);
        Check.That(imageAction.Player).Equals(players[0]);
        Check.That(imageAction.Latitude).IsEqualsWithDelta(59.3278160094444, 0.001);
        Check.That(imageAction.Longitude).IsEqualsWithDelta(18.0551338194444, 0.001);
        Check.That(imageAction.Picture).IsNotNull();
        Check.That(imageAction.Action).Equals(Action.SubmitPicture);
      }

      [Fact]
      public void GetPlayerById()
      {
        // Arrange
        var players = new List<Player> {new Player(), new Player(), new Player()};
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        var result = _target.GetPlayerById(players[1].Id);
        // Assert
        Check.That(result).Equals(players[1]);
      }

      [Fact]
      public void GetPlayerByChatId()
      {
      // Arrange
        var players = new List<Player> { new Player(){ChatLogin = "toto"}, new Player(){ChatLogin = "tata"}, new Player(){ChatLogin = "tutu"} };
        _context.Players.AddRange(players);
        _context.SaveChanges();
        // Act
        var result = _target.GetPlayerByChatId(players[1].ChatLogin);

        // Assert
        Check.That(result).Equals(players[1]);
      }
  }
}
