using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Services;
using NFluent;
using Xunit;

namespace ImageHuntTest.Services
{
    public class TeamServiceTest : ContextBasedTest
    {
        private TeamService _target;

        public TeamServiceTest()
        {
            _target = new TeamService(_context);
        }
        [Fact]
        public void CreateTeam()
        {
            // Arrange
            var team = new Team() { Name = "Team1" };
            // Act
            _target.CreateTeam(team);
            // Assert
            Check.That(_context.Teams.First()).IsEqualTo(team);
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
        // TODO Find a way to enable cascading delete in SQLite. Use foreign keys=true
        [Fact(Skip = "SQLite doesn't cascade delete")]
        public void DeleteTeamWithMembers()
        {
            // Arrange
            var teams = new List<Team>() { new Team(), new Team() { Players = new List<Player>() { new Player(), new Player() } }, new Team() };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            _target.DeleteTeam(teams[1]);
            // Assert
            Check.That(_context.Teams).ContainsExactly(teams[0], teams[2]);
            Check.That(_context.Players).HasSize(0);
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
            var teams = new List<Team>()
            {
                new Team(){Players = new List<Player>(){new Player(), new Player(), new Player()}},
                new Team(),
                new Team()
            };
            _context.Teams.AddRange(teams);
            _context.SaveChanges();
            // Act
            _target.DelMemberToTeam(teams[0], teams[0].Players[1]);
            // Assert
            Check.That(teams[0].Players).HasSize(2).And.ContainsExactly(teams[0].Players[0], teams[0].Players[1]);
        }
    }
}
