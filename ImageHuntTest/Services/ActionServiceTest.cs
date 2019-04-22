using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using Action = ImageHuntCore.Model.Action;

namespace ImageHuntTest.Services
{
    public class ActionServiceTest : ContextBasedTest<HuntContext>
    {
        private ActionService _target;
        private ILogger<ActionService> _logger;
        private IScoreChanger _scoreChanger;

        public ActionServiceTest()
        {
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<ActionService>>());
            TestContainerBuilder.RegisterInstance(Context);
            TestContainerBuilder.RegisterInstance(_scoreChanger = A.Fake<IScoreChanger>());
            var container = TestContainerBuilder.Build();
            _target = new ActionService(Context, _logger, _scoreChanger);
        }
        [Fact]
        public async Task GetGameActionForGame()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 0, 10, IncludeAction.All);
            // Assert
            Check.That(results).ContainsExactly(gameActions[0], gameActions[1], gameActions[2]);

            Check.That(results[2].Node).IsNull();
            Check.That(results[2].Delta).IsNaN();
            Check.That(results[0].Delta).IsEqualsWithDelta(15.6238, 0.001);
        }
        [Fact]
        public async Task GetGameActionForGame_WithPictureNode()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){
          new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
          new ObjectNode(){Latitude = 12, Longitude = 16},
          new LastNode(),
          new PictureNode() {Image = new Picture(){Image = new byte[10]}}
      };
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Node = nodes[3],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[3],
          Team = teams[1]
        },
      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 0, 10, IncludeAction.All);
            // Assert
            Check.That(results).ContainsExactly(gameActions[0], gameActions[1], gameActions[2]);

            Check.That(((PictureNode)results[2].Node).Image.Image).IsNull();
        }
        [Fact]
        public async Task GetGameActionForGame_Paginated()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 2, 2, IncludeAction.All);
            // Assert
            Check.That(results).ContainsExactly(gameActions[2]);

            Check.That(results[0].Node).IsNull();
            Check.That(results[0].Delta).IsNaN();
            Check.That(results[0].Delta).IsEqualsWithDelta(15.6238, 0.001);
        }
        [Fact]
        public async Task GetGameActionForGame_Paginated_With_Holes()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.SubmitPicture,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1],
          Picture = new Picture()
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 1, 2, IncludeAction.Picture);
            // Assert
            Check.That(results).HasSize(2);
            Check.That(results).ContainsExactly(gameActions[2], gameActions[4]);
            results = await _target.GetGameActionsForGame(games[1].Id, 2, 2, IncludeAction.Picture);
            Check.That(results).ContainsExactly(gameActions[5], gameActions[6]);
        }
        [Fact]
        public async Task GetGameActionForGame_Only_HiddenNodes()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.HiddenNode,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1],
          Picture = new Picture()
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.BonusNode,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

        new GameAction()
        {
          Game = games[0],
          Action = Action.BonusNode,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 1, 2, IncludeAction.HiddenNode);
            // Assert
            Check.That(results).HasSize(gameActions.Count(ga => ga.Game == games[1] && 
                                                                ( ga.Action == Action.BonusNode || ga.Action == Action.HiddenNode)));
            Check.That(results).ContainsExactly(gameActions[2], gameActions[4]);
        }
        [Fact]
        public async Task GetGameActionForGame_Only_QuestionNodes()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.ReplyQuestion,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1],
          Picture = new Picture()
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.ReplyQuestion,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

        new GameAction()
        {
          Game = games[0],
          Action = Action.ReplyQuestion,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },
          new GameAction()
          {
              Game = games[1],
              Action = Action.SubmitPicture,
              DateOccured = DateTime.Now,
              Latitude = 11,
              Longitude = 15.2,
              Node = nodes[1],
              Team = teams[1],
              Picture = new Picture()
          },

      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 1, 2, IncludeAction.ReplyQuestion);
            // Assert
            Check.That(results).HasSize(gameActions.Count(ga => ga.Game == games[1] && 
                                                                ( ga.Action == Action.ReplyQuestion)));
            Check.That(results).ContainsExactly(gameActions[2], gameActions[4]);
        }
        [Fact]
        public async Task GetGameActionForGame_Paginated_ForTeam()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
       new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[0]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[0]
        },
      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = await _target.GetGameActionsForGame(games[1].Id, 2, 2, IncludeAction.All, teams[1].Id);
            // Assert
            Check.That(results).ContainsExactly(gameActions[2]);

            Check.That(results[0].Node).IsNull();
            Check.That(results[0].Delta).IsNaN();
            Check.That(results[0].Delta).IsEqualsWithDelta(15.6238, 0.001);
        }

        [Fact]
        public void GetGameActionCountForGame()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
                new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
            {
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.StartGame,
                    DateOccured = DateTime.Now,
                    Latitude = 10,
                    Longitude = 15,
                    Node = nodes[0],
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now,
                    Latitude = 11,
                    Longitude = 15.2,
                    Node = nodes[1],
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now.AddMinutes(15),
                    Latitude = 11.3,
                    Longitude = 15.5,
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[0],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now,
                    Latitude = 11,
                    Longitude = 15.2,
                    Node = nodes[1],
                    Team = teams[1]
                },
            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var result = _target.GetGameActionCountForGame(games[1].Id, IncludeAction.All);
            // Assert
            Check.That(result).Equals(3);
        }
        [Fact]
        public void GetGameActionCountForGame_WithTeam()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
                new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
            {
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.StartGame,
                    DateOccured = DateTime.Now,
                    Latitude = 10,
                    Longitude = 15,
                    Node = nodes[0],
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now,
                    Latitude = 11,
                    Longitude = 15.2,
                    Node = nodes[1],
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now.AddMinutes(15),
                    Latitude = 11.3,
                    Longitude = 15.5,
                    Team = teams[1]
                },
                new GameAction()
                {
                    Game = games[1],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now.AddMinutes(15),
                    Latitude = 11.3,
                    Longitude = 15.5,
                    Team = teams[0]
                },
                new GameAction()
                {
                    Game = games[0],
                    Action = Action.VisitWaypoint,
                    DateOccured = DateTime.Now,
                    Latitude = 11,
                    Longitude = 15.2,
                    Node = nodes[1],
                    Team = teams[1]
                },
            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var result = _target.GetGameActionCountForGame(games[1].Id, IncludeAction.All, teams[1].Id);
            // Assert
            Check.That(result).Equals(3);
        }
        [Fact]
        public void GetGameAction()
        {
            // Arrange
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            Context.SaveChanges();
            var games = new List<Game> { new Game(), new Game() { Name = "Toto" } };
            Context.AddRange(games);
            Context.SaveChanges();
            var nodes = new List<Node>(){new FirstNode(){Latitude = 10.0001, Longitude = 15.0001},
        new ObjectNode(){Latitude = 12, Longitude = 16}, new LastNode()};
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameActions = new List<GameAction>()
      {
        new GameAction()
        {
          Game = games[1],
          Action = Action.StartGame,
          DateOccured = DateTime.Now,
          Latitude = 10,
          Longitude = 15,
          Node = nodes[0],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[1],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now.AddMinutes(15),
          Latitude = 11.3,
          Longitude = 15.5,
          Team = teams[1]
        },
        new GameAction()
        {
          Game = games[0],
          Action = Action.VisitWaypoint,
          DateOccured = DateTime.Now,
          Latitude = 11,
          Longitude = 15.2,
          Node = nodes[1],
          Team = teams[1]
        },
      };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var result = _target.GetGameAction(gameActions[1].Id);
            // Assert
            Check.That(result.Action).Equals(Action.VisitWaypoint);
            Check.That(result.Game.Name).Equals(games[1].Name);
            Check.That(result.Delta).IsEqualsWithDelta(141447.769119081, 0.001);
        }

        [Fact]
        public void AddGameAction()
        {
            // Arrange
            var games = new List<Game> { new Game(), new Game() };
            Context.Games.AddRange(games);
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            Context.Teams.AddRange(teams);
            var nodes = new List<Node> { new TimerNode(), new ObjectNode() };
            Context.Nodes.AddRange(nodes);
            Context.SaveChanges();
            var gameAction = new GameAction() { Game = games[1], Team = teams[1], Node = nodes[1] };
            // Act
            _target.AddGameAction(gameAction);
            // Assert
            Check.That(Context.GameActions).HasSize(1).And.Contains(gameAction);
        }

        [Fact]
        public void Validate_Validate()
        {
            // Arrange
            var gameActions = new List<GameAction> {new GameAction(),
            new GameAction() {IsValidated = false, Node = new PictureNode{Points = 15}}};
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            var admins = new List<Admin> { new Admin(), new Admin() };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            _target.Validate(gameActions[1].Id, gameActions[1].Node.Id, admins[1].Id, true);
            // Assert
            Check.That(gameActions.Extracting("IsValidated")).Contains(false, true);
            Check.That(gameActions.Extracting("IsReviewed")).Contains(false, true);
            Check.That(gameActions.Extracting("PointsEarned")).Contains(0, 15);
        }
        [Fact]
        public void Reject()
        {
            // Arrange
            var gameActions = new List<GameAction> {new GameAction(),
            new GameAction() {IsValidated = false, Node = new PictureNode{Points = 15}}};
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            var admins = new List<Admin> { new Admin(), new Admin() };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            _target.Validate(gameActions[1].Id, 1, admins[1].Id, false);
            // Assert
            Check.That(gameActions.Extracting("IsValidated")).Contains(false, false);
            Check.That(gameActions.Extracting("IsReviewed")).Contains(false, true);
            Check.That(gameActions.Extracting("PointsEarned")).Contains(0, 0);
        }

        [Fact]
        public void Validate_With_Node()
        {
            // Arrange
            var nodes = new List<Node> { new TimerNode() { Points = 15 }, new ObjectNode() { Points = 20 } };
            Context.Nodes.AddRange(nodes);
            var gameActions = new List<GameAction> { new GameAction(), new GameAction() { Node = nodes[1] } };
            Context.GameActions.AddRange(gameActions);
            var admins = new List<Admin> { new Admin(), new Admin() { Role = Role.Validator } };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            _target.Validate(gameActions[1].Id, nodes[1].Id, admins[1].Id, true);
            // Assert
            Check.That(gameActions.Extracting("PointsEarned")).Contains(0, 20);
        }
        [Fact]
        public void Validate_InValidate()
        {
            // Arrange
            var gameActions = new List<GameAction> {new GameAction(),
            new GameAction() {IsValidated = true, PointsEarned = 15,
                Node = new PictureNode(){Points = 15}}};
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            var admins = new List<Admin> { new Admin(), new Admin() };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            _target.Validate(gameActions[1].Id, gameActions[1].Node.Id, admins[1].Id, false);
            // Assert
            Check.That(gameActions.Extracting("IsValidated")).ContainsExactly(false, false);
            Check.That(gameActions.Extracting("IsReviewed")).ContainsExactly(false, true);
            Check.That(gameActions.Extracting("PointsEarned")).ContainsExactly(0, 0);
        }

        [Fact]
        public void Validate_without_Reviewer()
        {
            // Arrange
            var gameActions = new List<GameAction> { new GameAction(), new GameAction() { IsValidated = false } };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.Validate(gameActions[1].Id, 1, 0, true)).Throws<InvalidOperationException>();
            // Assert
        }

        [Fact]
        public void GetScoreForGame()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
                new Player(),
            };
            var games = new List<Game> { new Game() { NbPlayerPenaltyThreshold = 3, NbPlayerPenaltyValue = 0.05 } };
            Context.Games.AddRange(games);
            var teams = new List<Team> { new Team(), new Team(), new Team() };
            teams[0].TeamPlayers.Add(new TeamPlayer() { Team = teams[0], Player = players[0] });
            teams[0].TeamPlayers.Add(new TeamPlayer() { Team = teams[0], Player = players[1] });

            teams[1].TeamPlayers.Add(new TeamPlayer() { Team = teams[1], Player = players[2] });
            teams[1].TeamPlayers.Add(new TeamPlayer() { Team = teams[1], Player = players[3] });
            teams[1].TeamPlayers.Add(new TeamPlayer() { Team = teams[1], Player = players[4] });

            teams[2].TeamPlayers.Add(new TeamPlayer() { Team = teams[2], Player = players[5] });
            teams[2].TeamPlayers.Add(new TeamPlayer() { Team = teams[2], Player = players[6] });
            teams[2].TeamPlayers.Add(new TeamPlayer() { Team = teams[2], Player = players[7] });
            teams[2].TeamPlayers.Add(new TeamPlayer() { Team = teams[2], Player = players[8] });
            Context.Teams.AddRange(teams);
            var gameActions = new List<GameAction>
            {
                new GameAction{ Action=Action.StartGame, Game = games[0], Team = teams[0], DateOccured = DateTime.Now.AddHours(-1)},
                new GameAction{ Action=Action.StartGame, Game = games[0], Team = teams[1], DateOccured = DateTime.Now.AddHours(-2)},
                new GameAction{ Action=Action.StartGame, Game = games[0], Team = teams[2], DateOccured = DateTime.Now.AddHours(-3)},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[0], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[1], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[0], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[1], IsValidated = false, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[1], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[1], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[1], IsValidated = false, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[2], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[2], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[2], IsValidated = true, PointsEarned = 15},
                new GameAction {Action=Action.SubmitPicture, Game = games[0], Team = teams[2], IsValidated = true, PointsEarned = 15},
                new GameAction{ Action=Action.EndGame, Game = games[0], Team = teams[0], DateOccured = DateTime.Now.AddHours(1)},
                new GameAction{ Action=Action.EndGame, Game = games[0], Team = teams[1], DateOccured = DateTime.Now.AddHours(2)},
                new GameAction{ Action=Action.EndGame, Game = games[0], Team = teams[2], DateOccured = DateTime.Now.AddHours(3)},

            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            var expectedScores = new List<Score>
            {
                new Score(){Team = teams[0], Points = 30, TravelTime = new TimeSpan(2, 0, 0)},
                new Score(){Team = teams[1], Points = 45, TravelTime = new TimeSpan(4, 0, 0)},
                new Score(){Team = teams[2], Points = 57, TravelTime = new TimeSpan(6, 0, 0)},
            };

            A.CallTo(() => _scoreChanger.ComputeScore(A<Score>._, A<Game>._)).ReturnsNextFromSequence(expectedScores.Select(e => e.Points).ToArray());
            // Act
            var result = _target.GetScoresForGame(gameActions[1].Game.Id);
            // Assert
            var list = result.ToList();
            Check.That(result.Extracting("Points")).ContainsExactly(expectedScores.Extracting("Points"));
            Check.That(result.Extracting("Team")).ContainsExactly(expectedScores.Extracting("Team"));
            Check.That(result.First().TravelTime.Hours).IsEqualTo(expectedScores[0].TravelTime.Hours);
            Check.That(result.Last().TravelTime.Hours).IsEqualTo(expectedScores[2].TravelTime.Hours);
        }

        [Fact]
        public void GetPositionsForGame()
        {
            var teams = new List<Team>() { new Team(), new Team() };
            Context.Teams.AddRange(teams);

            var games = new List<Game>() { new Game(), new Game() };
            Context.Games.AddRange(games);
            // Arrange
            var gameActions = new List<GameAction>
            {
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.DoAction, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = _target.GetGamePositionsForGame(games[1].Id);
            // Assert
            Check.That(results).HasSize(7).And.Contains(gameActions[0], gameActions[1], gameActions[3], gameActions[4]);
        }
        [Fact]
        public void GetPositionsForGame_Null_Team_Should_be_ignored()
        {
            var teams = new List<Team>() {new Team(), new Team()};
            Context.Teams.AddRange(teams);
            var games = new List<Game>() { new Game(), new Game() };
            Context.Games.AddRange(games);
            // Arrange
            var gameActions = new List<GameAction>
            {
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.DoAction, Game = games[1]},
                new GameAction(){Team=teams[1], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Team=teams[1], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[1], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[1], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = _target.GetGamePositionsForGame(games[1].Id);
            // Assert
            Check.That(results).ContainsExactly(gameActions[0],gameActions[1],gameActions[3],gameActions[7],gameActions[8] );
        }
        [Fact]
        public void GetPositionsForGame_Null_Latitude_Should_Be_Ignored()
        {
            var teams = new List<Team>() { new Team(), new Team() };
            Context.Teams.AddRange(teams);

            var games = new List<Game>() { new Game(), new Game() };
            Context.Games.AddRange(games);
            // Arrange
            var gameActions = new List<GameAction>
            {
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.DoAction, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Team=teams[0], Latitude = null, Longitude=null, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[0]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
                new GameAction(){Team=teams[0], Latitude = 43.88, Longitude=4.86, Action=Action.SubmitPosition, Game = games[1]},
            };
            Context.GameActions.AddRange(gameActions);
            Context.SaveChanges();
            // Act
            var results = _target.GetGamePositionsForGame(games[1].Id);
            // Assert
            Check.That(results).HasSize(6).And.Contains(gameActions[0], gameActions[1], gameActions[4]);
        }

        [Fact]
        public void Should_Update_Modify_GameAction()
        {
            // Arrange
            var actions = new List<GameAction>
            {
                new GameAction() {PointsEarned = 56},
                new GameAction() {PointsEarned = 54},
                new GameAction() {PointsEarned = 34},
            };
            Context.GameActions.AddRange(actions);
            Context.SaveChanges();
            actions[1].PointsEarned = 457;
            // Act
            var modified = _target.Update(actions[1]);
            // Assert
            Check.That(modified.PointsEarned).Equals(actions[1].PointsEarned);
        }

        [Fact]
        public void Should_Next_Return_Next_Action_Not_reviewed_of_game()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game(),
                new Game(),
            };
            Context.Games.AddRange(games);
            var actions = new List<GameAction>
            {
                new GameAction() {PointsEarned = 56, Game = games[0], IsReviewed = true},
                new GameAction() {PointsEarned = 54, Game = games[1], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[0], IsReviewed = true},
                new GameAction() {PointsEarned = 54, Game = games[1], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[0], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[0], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[1], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[0], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[1], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[0], IsReviewed = false},
                new GameAction() {PointsEarned = 54, Game = games[1], IsReviewed = false},
                new GameAction() {PointsEarned = 34, Game = games[0], IsReviewed = false},
            };
            Context.GameActions.AddRange(actions);
            Context.SaveChanges();
            // Act
            var result = _target.GetNextGameAction(games[0].Id, actions[4].Id);
            // Assert
            Check.That(result).Equals(actions[5]);
        }
    }
}
