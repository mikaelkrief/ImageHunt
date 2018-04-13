using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using Action = System.Action;

namespace ImageHuntTest.Services
{
    public class ActionServiceTest : ContextBasedTest
    {
      private ActionService _target;
      private ILogger<ActionService> _logger;

      public ActionServiceTest()
      {
        _logger = A.Fake<ILogger<ActionService>>();
        _target = new ActionService(_context, _logger);
      }
    [Fact]
    public void GetGameActionForGame()
    {
      // Arrange
      var players = new List<Player> { new Player(), new Player() };
      _context.Players.AddRange(players);
      _context.SaveChanges();
      var games = new List<Game> { new Game(), new Game() };
      _context.AddRange(games);
      _context.SaveChanges();
      var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
      _context.Nodes.AddRange(nodes);
      _context.SaveChanges();
      var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Player = players[1]
        },
      };
      _context.GameActions.AddRange(gameActions);
      _context.SaveChanges();
      // Act
      var results = _target.GetGameActionsForGame(games[1].Id).ToList();
      // Assert
      Check.That(results).ContainsExactly(gameActions[0], gameActions[1], gameActions[2]);

      Check.That(results[2].Node).IsNull();
      Check.That(results[2].Delta).IsNaN();
      Check.That(results[0].Delta).IsEqualsWithDelta(15.6238, 0.001);
    }

    [Fact]
    public void GetGameAction()
    {
      // Arrange
      var players = new List<Player> { new Player(), new Player() };
      _context.Players.AddRange(players);
      _context.SaveChanges();
      var games = new List<Game> { new Game(), new Game() { Name = "Toto" } };
      _context.AddRange(games);
      _context.SaveChanges();
      var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
        new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
      _context.Nodes.AddRange(nodes);
      _context.SaveChanges();
      var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Player = players[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Player = players[1]
        },
      };
      _context.GameActions.AddRange(gameActions);
      _context.SaveChanges();
      // Act
      var result = _target.GetGameAction(gameActions[1].Id);
      // Assert
      Check.That(result.Action).Equals(ImageHunt.Model.Action.VisitWaypoint);
      Check.That(result.Game.Name).Equals(games[1].Name);
      Check.That(result.Delta).IsEqualsWithDelta(141447.769119081, 0.001);
    }

      [Fact]
      public void AddGameAction()
      {
        // Arrange
        var games = new List<Game> {new Game(), new Game()};
        _context.Games.AddRange(games);
        var players = new List<Player> {new Player(), new Player(), new Player()};
        _context.SaveChanges();
        var gameAction = new GameAction(){Game = games[1], Player = players[1]};
        // Act
        _target.AddGameAction(gameAction);
        // Assert
        Check.That(_context.GameActions).HasSize(1).And.Contains(gameAction);
      }
  }
}
