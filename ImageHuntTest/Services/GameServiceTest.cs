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
            var nodes = new List<Node>() { new FirstNode(), new TimerNode(), new TimerNode(), new TimerNode(), new QuestionNode() };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var result = _target.CreateGame("TheGame", DateTime.Today, nodes);
            // Assert
            Check.That(result.Id).Not.IsEqualTo(0);
            Check.That(result.StartDate).IsEqualTo(DateTime.Today);
            Check.That(result.Nodes).ContainsExactly(nodes);
        }

        [Fact]
        public void CreateGameFirstNodeNotInNodes()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode(), new QuestionNode() };
            // Act
            Check.ThatCode(() => _target.CreateGame("TheGame", DateTime.Today, nodes)).Throws<ArgumentException>();
            // Assert
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

    }
}
