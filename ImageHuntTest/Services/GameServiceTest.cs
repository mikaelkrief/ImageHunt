using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Helpers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using NFluent;
using SQLitePCL;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    [Collection("AutomapperFixture")]

    public class GameServiceTest : ContextBasedTest<HuntContext>
  {
    private GameService _target;
    private ILogger<GameService> _logger;
      private IMapper _mapper;

      public GameServiceTest()
    {
      _logger = A.Fake<ILogger<GameService>>();
        _mapper = Mapper.Instance;
      _target = new GameService(_context, _logger, _mapper);
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
        Check.That(result.Code).IsNotEmpty();
    }
      [Fact]
    public void CreateGame_With_Picture()
    {
      // Arrange
      var admins = new List<Admin>() { new Admin(), new Admin(), new Admin() };
      _context.Admins.AddRange(admins);
        var pictures = new List<Picture>() {new Picture(), new Picture()};
        _context.Pictures.AddRange(pictures);
      _context.SaveChanges();

      var game = new Game(){Picture = new Picture(){Id = pictures[1].Id} };

      // Act
      var result = _target.CreateGame(admins[1].Id, game);
      // Assert
      Check.That(result.Id).Not.IsEqualTo(0);
      Check.That(admins[1].Games).ContainsExactly(game);
    }

    [Fact]
    public void DeleteGame()
    {
      // Arrange
      var games = new List<Game>()
      {
        new Game(),
        new Game(),
        new Game(),
      };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      _target.DeleteGame(games[1].Id);
      // Assert
      Check.That(_context.Games).HasSize(2);
    }


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
      public void GetQuestionNodeOfGame()
      {
          // Arrange
          var answers = new List<Answer>(){new Answer(), new Answer(), new Answer(), new Answer()};
          var nodes = new List<Node>()
          {
              new TimerNode(),
              new ChoiceNode() {Answers = new List<Answer>() {answers[0], answers[1]}},
              new PictureNode(),
              new ChoiceNode() {Answers = new List<Answer>() {answers[2], answers[3]}},
              new ChoiceNode()
          };
            var games = new List<Game>() {new Game(), new Game(){Nodes = nodes.Take(4).ToList()}};
            _context.Games.AddRange(games);
          _context.SaveChanges();
          // Act
          var result = _target.GetChoiceNodeOfGame(games[1].Id);
          // Assert
          Check.That(result).ContainsExactly(nodes[1], nodes[3]);
          Check.That(result.First().Answers).ContainsExactly(answers[0], answers[1]);
          Check.That(result.Last().Answers).ContainsExactly(answers[2], answers[3]);
          
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
      var admin = new Admin() { };
        admin.GameAdmins = new List<GameAdmin>(){new GameAdmin(){Game = games[0], Admin = admin},new GameAdmin(){Game = games[1], Admin = admin},};
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
    public void GetHiddenNodes()
    {
      // Arrange
      var nodes = new List<Node>()
      {
          new TimerNode() { Name = "First" },
          new HiddenNode() { Name = "Second" },
          new TimerNode() { Name = "Third" },
          new BonusNode() { Name = "Fourth" }
      };
      var games = new List<Game>() { new Game(), new Game() { Nodes = nodes } };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var resNodes = _target.GetNodes(games[1].Id, NodeTypes.Hidden);
      // Assert
      Check.That(resNodes).ContainsExactly(nodes[1], nodes[3]);
    }
    [Fact]
    public void GetPictureNodes()
    {
      // Arrange
      var nodes = new List<Node>()
      {
          new TimerNode() { Name = "First" },
          new HiddenNode() { Name = "Second" },
          new TimerNode() { Name = "Third" },
          new BonusNode() { Name = "Fourth" },
          new PictureNode() { Name = "Five" }
      };
      var games = new List<Game>() { new Game(), new Game() { Nodes = nodes } };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var resNodes = _target.GetNodes(games[1].Id, NodeTypes.Picture);
      // Assert
      Check.That(resNodes).ContainsExactly(nodes[4]);
    }
    [Fact]
    public void SetCenterOfGameByNodes()
    {
      // Arrange
      var nodes = new List<Node>()
        {
          new PictureNode(){Latitude = 48.8501065, Longitude = 2.327722},
          new TimerNode(){Latitude = 48.851291, Longitude = 2.3318698},
          new ChoiceNode(){Latitude = 48.8537828, Longitude = 2.3310879}
        };
      var games = new List<Game>() { new Game(), new Game() { Nodes = nodes, MapZoom = 10} };
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      _target.SetCenterOfGameByNodes(games[1].Id);
      // Assert
      Check.That(games[1].MapCenterLat.Value).IsEqualsWithDelta(48.8517267806692, 0.0001);
      Check.That(games[1].MapCenterLng.Value).IsEqualsWithDelta(2.33022653262665, 0.0001);
      Check.That(_context.Games.Last().MapZoom).Equals(10);
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
      var players = new List<Player>()
        {
          new Player() {ChatLogin = "toto"},
          new Player() {ChatLogin = "Titi"},
          new Player() {ChatLogin = "tata"}
        };
      var teams = new List<Team>() { new Team(), new Team()};
      var teamPlayers = players.Select(p => new TeamPlayer() {Team = teams[1], Player = p});
      teams[1].TeamPlayers = teamPlayers.ToList();
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

    [Fact]
    public void GetImagesForGame()
    {
      // Arrange
      var nodes = new List<Node>
      {
        new FirstNode(),
        new PictureNode(){Image = new Picture(){Id=13, Image = new byte[56]}},
        new ObjectNode(),
        new PictureNode(){Image = new Picture(){Id=13, Image = new byte[56]}},
        new ChoiceNode()
      };
      var games = new List<Game> {new Game(), new Game(){Nodes = nodes}, new Game()};
      _context.Games.AddRange(games);
      _context.SaveChanges();
      // Act
      var result = _target.GetPictureNode(games[1].Id);
      // Assert
      Check.That(result).Contains(nodes[1], nodes[3]);
    }

      [Fact]
      public void GetGamesWithScore()
      {
          // Arrange
          var games = new List<Game>
          {
              new Game() {StartDate = DateTime.Now.AddHours(-1)},
              new Game() {StartDate = DateTime.Now.AddDays(-1)},
              new Game() {StartDate = DateTime.Now.AddDays(1)},
              new Game() {StartDate = DateTime.Now.AddDays(-1)},
          };
          _context.AddRange(games);
          _context.SaveChanges();
          var gameActions = new List<GameAction>
          {
              new GameAction() {Game = games[0], IsReviewed = true, PointsEarned = 15},
              new GameAction() {Game = games[0], IsReviewed = true, PointsEarned = 6},
              new GameAction() {Game = games[0], IsReviewed = true, PointsEarned = 0},
              new GameAction() {Game = games[0], IsReviewed = true, PointsEarned = 15},
              new GameAction() {Game = games[1], IsReviewed = true, PointsEarned = 155},
              new GameAction() {Game = games[1], IsReviewed = true, PointsEarned = 115},
              new GameAction() {Game = games[3], IsReviewed = false, PointsEarned = 155},
              new GameAction() {Game = games[3], IsReviewed = false, PointsEarned = 115},
          };
          _context.GameActions.AddRange(gameActions);
          _context.SaveChanges();
          // Act
          var results = _target.GetGamesWithScore();
          // Assert
          Check.That(results).ContainsExactly(games[0], games[1]);
      }

      [Fact]
      public void GetActiveGameForPlayer()
      {
          var players = new List<Player>
          {
              new Player(),new Player(),new Player(),new Player(), new Player(),
              new Player(),new Player(),new Player(),new Player(), new Player(),
              new Player(),new Player(),new Player(),new Player(), new Player(),
              new Player(),new Player(),new Player(),new Player(), new Player(),
          };
          _context.Players.AddRange(players);
          _context.SaveChanges();
          var teams = new List<Team>
          {
              new Team(),
              new Team(),
              new Team(),
              new Team(),
          };
          teams[0].TeamPlayers=new List<TeamPlayer>
          {
              new TeamPlayer(){Player = players[0], Team = teams[0]},
              new TeamPlayer(){Player = players[1], Team = teams[0]},
              new TeamPlayer(){Player = players[3], Team = teams[0]},
          };
          teams[1].TeamPlayers=new List<TeamPlayer>
          {
              new TeamPlayer(){Player = players[2], Team = teams[1]},
              new TeamPlayer(){Player = players[4], Team = teams[1]},
              new TeamPlayer(){Player = players[6], Team = teams[1]},
          };
          teams[2].TeamPlayers=new List<TeamPlayer>
          {
              new TeamPlayer(){Player = players[0], Team = teams[2]},
              new TeamPlayer(){Player = players[7], Team = teams[2]},
              new TeamPlayer(){Player = players[8], Team = teams[2]},
          };
          teams[3].TeamPlayers=new List<TeamPlayer>
          {
              new TeamPlayer(){Player = players[8], Team = teams[3]},
              new TeamPlayer(){Player = players[6], Team = teams[3]},
              new TeamPlayer(){Player = players[9], Team = teams[3]},
          };
          _context.Teams.AddRange(teams);
          _context.SaveChanges();
          // Arrange
          var games = new List<Game>
          {
              new Game(){IsActive = false},
              new Game(){IsActive = true, StartDate = DateTime.Today, Teams = new List<Team>(){teams[0], teams[2]}},
              new Game(){IsActive = true, StartDate = DateTime.Today.AddDays(1), Teams = new List<Team>(){teams[0], teams[2]}},
              new Game(){IsActive = true, StartDate = DateTime.Today, Teams = new List<Team>(){teams[1], teams[3]}},
          };
          games[0].Teams.Add(teams[0]);
          games[0].Teams.Add(teams[2]);
          games[1].Teams.Add(teams[0]);
          games[1].Teams.Add(teams[2]);
          games[2].Teams.Add(teams[0]);
          games[2].Teams.Add(teams[2]);
          games[3].Teams.Add(teams[1]);
          games[3].Teams.Add(teams[3]);
          _context.Games.AddRange(games);
          _context.SaveChanges();
          // Act
          var result = _target.GetActiveGameForPlayer(players[0]);
          // Assert
          Check.That(result).Equals(games[1]);
      }
      [Fact]
      public void GetAllGame()
      {
          // Arrange
          var games = new List<Game>
          {
              new Game(),
              new Game() {IsActive = false},
              new Game(),
              new Game(),
          };
            _context.Games.AddRange(games);
          _context.SaveChanges();
          // Act
          var result = _target.GetAllGame();
          // Assert
          Check.That(result).Contains(games.Where(g => g.IsActive));
      }

      [Fact]
      public void Should_Game_Code_Create_Code()
      {
          // Arrange
          var games = new List<Game>
          {
              new Game(),
              new Game(),
              new Game(),
          };
          _context.Games.AddRange(games);
          _context.SaveChanges();

            // Act
            var result = _target.GameCode(games[1].Id);
          // Assert
          Check.That(result).IsNotEmpty();
          Check.That(result).Equals(games[1].Code);
      }
      [Fact]
      public void Should_Game_Code_return_Code()
      {
          // Arrange
          var games = new List<Game>
          {
              new Game(),
              new Game(){Code = "jhgjhgjhg"},
              new Game(),
          };
          _context.Games.AddRange(games);
          _context.SaveChanges();
          // Act
          var result = _target.GameCode(games[1].Id);
          // Assert
          Check.That(result).Equals(games[1].Code);
      }

      [Fact]
      public void Should_Duplicate_Succeed()
      {
          // Arrange
          var nodes = new List<Node>
          {
              new FirstNode(),
              new ObjectNode(),
              new TimerNode(),
              new QuestionNode(),
              new LastNode()
          };
          nodes[0].HaveChild(nodes[1]);
          nodes[1].HaveChild(nodes[2]);
          nodes[2].HaveChild(nodes[3]);
          nodes[3].HaveChild(nodes[4]);
          var games = new List<Game>
          {
              new Game(),
              new Game(){Nodes = nodes}
          };
          _context.Games.AddRange(games);
          var admins = new List<Admin>
          {
              new Admin(),
              new Admin(),
          };
          admins[1].GameAdmins.Add(new GameAdmin(){Admin = admins[1], Game = games[1]});
          _context.Admins.AddRange(admins);
            _context.SaveChanges();
          // Act
          var newGame = _target.Duplicate(games[1], admins[1]);
          // Assert
          Check.That(newGame).IsNotNull();
          Check.That(_context.Games).HasSize(3);
      }
      [Fact]
      public void Should_GetGameByCode_Return_Game()
      {
          // Arrange
          var teams = new List<Team>
          {
              new Team(),
              new Team(),
              new Team(),
              new Team(),
              new Team(),
          };
          _context.Teams.AddRange(teams);
          var games = new List<Game>
          {
              new Game() {Code = "AZERTY", Teams = new List<Team>(){teams[0], teams[2], teams[4]}},
              new Game() {Code = "HJGHG", Teams = new List<Team>(){teams[1], teams[3]}},
          };
          _context.Games.AddRange(games);
          _context.SaveChanges();
          // Act
          var game = _target.GetGameByCode(games[0].Code);
          // Assert
          Check.That(game).Equals(games[0]);
      }

    }
}
