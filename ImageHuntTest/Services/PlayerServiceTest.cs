using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Services;
using ImageHuntCore.Model;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest
{
    public class PlayerServiceTest : ContextBasedTest<HuntContext>
    {
        private PlayerService _target;
        private ILogger<PlayerService> _logger;

        public PlayerServiceTest()
        {
            _logger = A.Fake<ILogger<PlayerService>>();
            _target = new PlayerService(Context, _logger);
        }
        [Fact]
        public void CreatePlayerTest()
        {
            // Arrange
            var players = new List<Player>()
        {
          new Player() {Name = "Titi", ChatLogin = "@titi"}
        };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            _target.CreatePlayer("Toto", "chatLogin");
            // Assert
            Check.That(Context.Players).HasSize(2);
            BooleanCheckExtensions.IsTrue(Check.That(Context.Players.Any(p => p.Name == "Toto")));
        }

        [Fact]
        public void JoinTeam()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Team1" }, new Team() { Name = "Team2" } };
            Context.Teams.AddRange(teams);
            var games = new List<Game>() { new Game() { Name = "game1" }, new Game() { Name = "game2", Teams = new List<Team>() { teams[1] } } };
            Context.Games.AddRange(games);
            var players = new List<Player>() { new Player() { Name = "Toto" }, new Player() { Name = "Titi" } };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            _target.JoinTeam(teams[1].Id, players[0].Id);
            // Assert
            Check.That(teams[1].Players).ContainsExactly(players[0]);
        }
        [Fact]
        public void JoinTeam_TeamDoesntExist()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Team1" }, new Team() { Name = "Team2" } };
            Context.Teams.AddRange(teams);
            var games = new List<Game>() { new Game() { Name = "game1" }, new Game() { Name = "game2", Teams = new List<Team>() { teams[1] } } };
            Context.Games.AddRange(games);
            var players = new List<Player>() { new Player() { Name = "Toto" }, new Player() { Name = "Titi" } };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.JoinTeam(-1, players[0].Id)).Throws<InvalidOperationException>();
            // Assert
        }
        [Fact]
        public void JoinTeam_PlayerDoesntExist()
        {
            // Arrange
            var teams = new List<Team>() { new Team() { Name = "Team1" }, new Team() { Name = "Team2" } };
            Context.Teams.AddRange(teams);
            var games = new List<Game>() { new Game() { Name = "game1" }, new Game() { Name = "game2", Teams = new List<Team>() { teams[1] } } };
            Context.Games.AddRange(games);
            var players = new List<Player>() { new Player() { Name = "Toto" }, new Player() { Name = "Titi" } };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.JoinTeam(players[1].Id, -1)).Throws<InvalidOperationException>();
            // Assert
            Check.That(teams[1].Players).HasSize(0);
        }


        [Fact]
        public void GetPlayerById()
        {
            // Arrange
            var players = new List<Player> { new Player(), new Player(), new Player() };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            var result = _target.GetPlayerById(players[1].Id);
            // Assert
            Check.That(result).Equals(players[1]);
        }

        [Fact]
        public void GetPlayerByChatId()
        {
            // Arrange
            var players = new List<Player> { new Player() { ChatLogin = "toto" }, new Player() { ChatLogin = "tata" }, new Player() { ChatLogin = "tutu" } };
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            var result = _target.GetPlayerByChatId(players[1].ChatLogin);

            // Assert
            Check.That(result).Equals(players[1]);
        }
    }
}
