using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
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
            var admins = new List<Admin>(){new Admin(), new Admin(), new Admin()};
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
            var admin = new Admin() {Games = games};
            _context.Admins.Add(admin);
            _context.SaveChanges();
            // Act
            var results = _target.GetGamesForAdmin(admin.Id);
            // Assert
            Check.That(results).ContainsExactly(games);
        }
    }
}
