using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Components.DictionaryAdapter;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Exception;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using Action = ImageHuntWebServiceClient.Action;

namespace ImageHuntTest.Services
{
    public class TeamServiceTest : ContextBasedTest<HuntContext>
    {
        private TeamService _target;
        private ILogger<TeamService> _logger;

        public TeamServiceTest()
        {
            _logger = A.Fake<ILogger<TeamService>>();
            _target = new TeamService(_context, _logger);
        }
        [Fact]
        public void CreateTeam()
        {
            // Arrange
            var games = new List<Game>() { new Game(), new Game() };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            var team = new Team() { Name = "Team1" };
            // Act
            _target.CreateTeam(games[1].Id, team);
            // Assert
            Check.That(games[1].Teams.First()).IsEqualTo(team);
            Check.That(team.Id).IsNotZero();
        }

        [Fact]
        public void DeleteTeam()
        {
            // Arrange
            var teams = new List<Team>() { new Team(), new Team(), new Team() };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            _target.DeleteTeam(teams[1]);
            // Assert
            Check.That(_context.Teams).ContainsExactly(teams[0], teams[2]);
        }

        [Fact]
        public void GetTeams()
        {
            // Arrange
            var teams = new List<Team>() { new Team(), new Team(), new Team() };
            var games = new List<Game>() { new Game() { Teams = teams }, new Game() };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.GetTeams(games[0].Id);
            // Assert
            Check.That(result).ContainsExactly(teams);
        }

        [Fact]
        public void GetTeamByName()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Team1" }, new Team() { Name = "Team2" }, new Team() { Name = "Team3" } };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            var team = _target.GetTeamByName("Team2");
            // Assert
            Check.That(team).IsEqualTo(teams[1]);
        }
        [Fact]
        public void GetTeamById()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Team1" }, new Team() { Name = "Team2" }, new Team() { Name = "Team3" } };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            var team = _target.GetTeamById(teams[1].Id);
            // Assert
            Check.That(team).IsEqualTo(teams[1]);
        }

        [Fact]
        public void AddMemberToTeam()
        {
            // Arrange
            var teams = new List<Team>() { new Team(), new Team(), new Team() };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            var players = new List<Player>() { new Player(), new Player(), new Player() };
            // Act
            _target.AddMemberToTeam(teams[1], players);
            // Assert
            Check.That(teams[1].Players).ContainsExactly(players);
            Check.That(_context.Players).ContainsExactly(players);
        }

        [Fact]
        public void DelMemberToTeam()
        {
            // Arrange
            var players = new List<Player>() { new Player(), new Player(), new Player() };
            _context.Players.AddRange(players);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team()
            };
            _context.Teams.AddRange(teams);
 
            _context.SaveChanges();
            _target.AddMemberToTeam(teams[0], players);
            // Act
            _target.DelMemberToTeam(teams[0], players[1]);
            // Assert
            var team = _target.GetTeamById(teams[0].Id);

            Check.That(team.Players).HasSize(2);
        }


        [Fact]
        public void RemovePlayer()
        {
            // Arrange
            var players = new List<Player>() { new Player() { ChatLogin = "toto1" }, new Player() { ChatLogin = "toto2" }, new Player() { ChatLogin = "Toto3" } };
            var teams = new List<Team>(){new Team()
        {
          Name = "Team1"
        },

        new Team(){Name = "Team2"}

      };

            _context.Players.AddRange(players);
            _context.Teams.AddRange(teams);

            var teamPlayers = new List<TeamPlayer>
      {
        new TeamPlayer() {Team = teams[0], Player = players[0]},
        new TeamPlayer() {Team = teams[0], Player = players[2]},
        new TeamPlayer() {Team = teams[1], Player = players[1]},

      };
            teams[0].TeamPlayers = new List<TeamPlayer>() { teamPlayers[0], teamPlayers[1] };
            teams[1].TeamPlayers = new List<TeamPlayer>() { teamPlayers[2] };
            _context.SaveChanges();
            // Act
            _target.DelMemberToTeam(teams[0], players[2]);
            // Assert
            Check.That(teams[0].Players).HasSize(1);
        }

        [Fact]
        public void GetTeamsForPlayer()
        {
            // Arrange
            var players = new List<Player>() { new Player() { ChatLogin = "toto1" }, new Player() { ChatLogin = "toto2" }, new Player() { ChatLogin = "Toto3" } };
            var teams = new List<Team>(){new Team()
        {
          Name = "Team1"
        },

        new Team(){Name = "Team2"}

      };

            _context.Players.AddRange(players);
            _context.Teams.AddRange(teams);

            var teamPlayers = new List<TeamPlayer>
      {
        new TeamPlayer() {Team = teams[0], Player = players[0]},
        new TeamPlayer() {Team = teams[0], Player = players[2]},
        new TeamPlayer() {Team = teams[1], Player = players[1]},

      };
            teams[0].TeamPlayers = new List<TeamPlayer>() { teamPlayers[0], teamPlayers[1] };
            teams[1].TeamPlayers = new List<TeamPlayer>() { teamPlayers[2] };
            _context.SaveChanges();
            // Act
            var result = _target.GetTeamsForPlayer(players[1]);
            // Assert
            Check.That(result).Contains(teams[1]);
        }
        [Fact]
        public void StartPlayer_PlayerDoesntExist()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Toto" } };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.StartGame(1, 0)).Throws<InvalidOperationException>();
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
        public void StartTeam()
        {
            // Arrange
            var players = new List<Player>() { new Player() { Name = "Toto" } };
            _context.Players.AddRange(players);
            var teams = new List<Team>() { new Team() };
            var teamPlayers = players.Select(p => new TeamPlayer() { Player = p, Team = teams[0] });
            teams[0].TeamPlayers = teamPlayers.ToList();
            var nodes = new List<Node>() { new FirstNode() { Latitude = 10, Longitude = 12 } };
            var games = new List<Game>() { new Game() { Teams = teams, IsActive = true, Name = "Game1", StartDate = DateTime.Now, Nodes = nodes } };
            players[0].CurrentGame = games[0];
            _context.Nodes.AddRange(nodes);
            _context.Teams.AddRange(teams);
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            _target.StartGame(1, teams[0].Id);
            // Assert
            Check.That(teams[0].CurrentNode).Equals(nodes[0]);
        }
        [Fact]
        public void StartPlayer_GameNotActive()
        {
            // Arrange
            var players = new List<Player>() { new Player() { Name = "Toto" } };
            _context.Players.AddRange(players);
            var teams = new List<Team>() { new Team() };
            var teamPlayers = players.Select(p => new TeamPlayer() { Player = p, Team = teams[0] });
            teams[0].TeamPlayers = teamPlayers.ToList();
            var nodes = new List<Node>() { new FirstNode() { Latitude = 10, Longitude = 12 } };
            var games = new List<Game>() { new Game() { Teams = teams, IsActive = false, Name = "Game1", StartDate = DateTime.Now, Nodes = nodes } };
            players[0].CurrentGame = games[0];
            _context.Nodes.AddRange(nodes);
            _context.Teams.AddRange(teams);
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.StartGame(games[0].Id, teams[0].Id)).Throws<ArgumentException>();
            // Assert
        }

        [Fact]
        public void NextNodeForPlayer()
        {
            // Arrange
            var nodes = new List<Node>() { new FirstNode() { Latitude = 10, Longitude = 11 }, new ObjectNode(), new PictureNode() };
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
            var teams = new List<Team> { new Team() { CurrentNode = nodes[0] }, new Team() };
            _context.Teams.AddRange(teams);
            games[0].Teams = teams;
            _context.SaveChanges();
            // Act
            var result = _target.NextNodeForTeam(teams[0].Id, 15, 16);
            // Assert
            Check.That(teams[0].CurrentNode).Equals(nodes[1]);
            Check.That(result).Equals(nodes[1]);
            Check.That(_context.GameActions).HasSize(1);
            Check.That(_context.GameActions.Extracting("Game")).Contains(games[0]);
            Check.That(_context.GameActions.Extracting("Team")).Contains(teams[0]);
            Check.That(_context.GameActions.Extracting("Latitude")).Contains(15);
            Check.That(_context.GameActions.Extracting("Longitude")).Contains(16);
            Check.That(_context.GameActions.Extracting("Node")).Contains(nodes[0]);
        }
        [Fact]
        public void NextNodeForPlayer_GameNotStarted()
        {
            // Arrange
            var nodes = new List<Node>() { new FirstNode(), new ObjectNode(), new PictureNode() };
            var childrenRelations = new List<ParentChildren>()
        {
          new ParentChildren(){Parent = nodes[0], Children = nodes[1]}
        };
            _context.ParentChildren.AddRange(childrenRelations);
            nodes[0].ChildrenRelation = childrenRelations;
            _context.Nodes.AddRange(nodes);
            var games = new List<Game>()
        {
          new Game() {Nodes = nodes, IsActive = false}
        };
            _context.Games.AddRange(games);
            var players = new List<Player>()
        {
          new Player() { Name = "Toto", CurrentGame = games[0]}
        };
            _context.Players.AddRange(players);
            var teams = new List<Team> { new Team(), new Team() };
            _context.Teams.AddRange(teams);
            games[0].Teams = teams;
            _context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.NextNodeForTeam(teams[0].Id, 0, 0)).Throws<InvalidGameException>();
            // Assert
        }
        [Fact]
        public void NextNodeForPlayer_FirstNode()
        {
            // Arrange
            var nodes = new List<Node>() { new FirstNode(), new ObjectNode(), new PictureNode() };
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
        new Player() { Name = "Toto", CurrentGame = games[0]}
      };
            _context.Players.AddRange(players);
            var teams = new List<Team> { new Team(), new Team() };
            _context.Teams.AddRange(teams);
            games[0].Teams = teams;
            _context.SaveChanges();
            // Act
            var nextNode = _target.NextNodeForTeam(teams[0].Id, 0, 0);
            // Assert
            Check.That(nextNode).Equals(nodes[0]);
        }

        [Fact]
        public void UploadImage_PlayerDoesntExist()
        {
            // Arrange

            // Act
            Check.ThatCode(() => _target.UploadImage(1, 1, 15, 15, null)).Throws<ArgumentException>();
            // Assert
        }

        [Fact]
        public void UploadImage_NoImageProvided()
        {
            // Arrange
            var players = new List<Player>() { new Player() { Name = "Toto" } };
            _context.Players.AddRange(players);
            _context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.UploadImage(1, 1, 5, 5, null)).Throws<ArgumentException>();
            // Assert
        }

        [Fact]
        public void UploadImage()
        {
            // Arrange
            var players = new List<Player>() { new Player() { Name = "Toto" } };
            var teams = new List<Team> { new Team(), new Team() };
            _context.Teams.AddRange(teams);
            var image1 = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            var image2 = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.ingress_20180128_130017_1.jpg");
            var nodes = new List<Node>()
        {
          new PictureNode(){Image = new Picture(){Image = image1 }, Latitude = 59.3278160094000, Longitude = 18.0551338194444},
          new PictureNode(){Image = new Picture(){Image = image2}}
        };
            _context.Nodes.AddRange(nodes);
            var games = new List<Game>() { new Game() { Nodes = nodes, Teams = teams } };
            players[0].CurrentGame = games[0];
            _context.Players.AddRange(players);
            _context.SaveChanges();
            // Act
            _target.UploadImage(games[0].Id, teams[0].Id, 59.3278160094444, 18.0551338194444, image1);
            // Assert
            var imageAction = _context.GameActions.First();
            Check.That(imageAction.Game).Equals(games[0]);
            Check.That(imageAction.Team).Equals(teams[0]);
            Check.That(imageAction.Latitude.Value).IsEqualsWithDelta(59.3278160094444, 0.001);
            Check.That(imageAction.Longitude.Value).IsEqualsWithDelta(18.0551338194444, 0.001);
            Check.That(imageAction.Picture).IsNotNull();
            Check.That(imageAction.Action).Equals(Action.SubmitPicture);
            Check.That(imageAction.Node).Equals(nodes[0]);
        }

        [Fact]
        public void GetTeamForPlayer()
        {
            // Arrange
            var players = new List<Player>
          {
              new Player() {ChatLogin = "toto"},
              new Player() {ChatLogin = "titi"},
              new Player() {ChatLogin = "tata"},
          };
            _context.Players.AddRange(players);
            _context.SaveChanges();

            var teams = new List<Team>
          {
              new Team(),
              new Team(),
              new Team(),
          };
            teams[0].TeamPlayers = new List<TeamPlayer>() { new TeamPlayer() { Team = teams[0], Player = players[0] } };
            teams[1].TeamPlayers = new List<TeamPlayer>() { new TeamPlayer() { Team = teams[1], Player = players[1] } };
            teams[2].TeamPlayers = new List<TeamPlayer>() { new TeamPlayer() { Team = teams[2], Player = players[2] } };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();

            var games = new List<Game>
          {
              new Game() {Teams = new List<Team>() {teams[0], teams[1]}},
              new Game() {Teams = new List<Team>() {teams[2]}}
          };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.GetTeamForUserName(games[0].Id, players[1].ChatLogin);
            // Assert
            Check.That(result).Equals(teams[1]);
        }

    }
}
