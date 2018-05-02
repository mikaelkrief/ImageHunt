using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
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
      var teams = new List<Team> {new Team(), new Team(), new Team()};
      _context.Teams.AddRange(teams);
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
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
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
      var teams = new List<Team> { new Team(), new Team(), new Team() };
      _context.Teams.AddRange(teams);
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
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = ImageHunt.Model.Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
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
        var teams = new List<Team> { new Team(), new Team(), new Team() };
        _context.Teams.AddRange(teams);
        _context.SaveChanges();
        var gameAction = new GameAction(){Game = games[1], Team = teams[1]};
        // Act
        _target.AddGameAction(gameAction);
        // Assert
        Check.That(_context.GameActions).HasSize(1).And.Contains(gameAction);
      }

      [Fact]
      public void Validate_Validate()
      {
        // Arrange
        var gameActions = new List<GameAction> {new GameAction(), new GameAction() {IsValidated = false}};
        _context.GameActions.AddRange(gameActions);
        _context.SaveChanges();
        var admins = new List<Admin> {new Admin(), new Admin()};
        _context.Admins.AddRange(admins);
        _context.SaveChanges();
        // Act
        _target.Validate(gameActions[1].Id, admins[1].Id);
        // Assert
        Check.That(gameActions.Extracting("IsValidated")).Contains(false, true);
        Check.That(gameActions.Extracting("IsReviewed")).Contains(false, true);
      }

      [Fact]
      public void Validate_With_Node()
      {
        // Arrange
        var nodes = new List<Node> {new TimerNode() {Points = 15}, new ObjectNode(){Points = 20}};
        _context.Nodes.AddRange(nodes);
        var gameActions = new List<GameAction> { new GameAction(), new GameAction() { Node = nodes[1]} };
        _context.GameActions.AddRange(gameActions);
        var admins = new List<Admin> { new Admin(), new Admin(){Role = Role.Validator} };
        _context.Admins.AddRange(admins);
        _context.SaveChanges();
        // Act
        _target.Validate(gameActions[1].Id, admins[1].Id);
        // Assert
        Check.That(gameActions.Extracting("PointsEarned")).Contains(0, 20);
      }
      [Fact]
      public void Validate_InValidate()
      {
        // Arrange
        var gameActions = new List<GameAction> {new GameAction(), new GameAction() {IsValidated = true}};
        _context.GameActions.AddRange(gameActions);
        _context.SaveChanges();
      var admins = new List<Admin> { new Admin(), new Admin() };
        _context.Admins.AddRange(admins);
        _context.SaveChanges();
        // Act
      _target.Validate(gameActions[1].Id, admins[1].Id);
        // Assert
        Check.That(gameActions.Extracting("IsValidated")).Contains(false, false);
        Check.That(gameActions.Extracting("IsReviewed")).Contains(false, true);
      }

    [Fact]
      public void Validate_without_Reviewer()
      {
      // Arrange
        var gameActions = new List<GameAction> { new GameAction(), new GameAction() { IsValidated = false } };
        _context.GameActions.AddRange(gameActions);
        _context.SaveChanges();
        // Act
        Check.ThatCode(()=> _target.Validate(gameActions[1].Id, 0)).Throws<InvalidOperationException>();
        // Assert
      }
  }
}
