using System;
using System.Collections.Generic;
using System.Text;
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
            var nodes = new List<Node>(){new TimerNode(), new TimerNode(), new TimerNode(), new QuestionNode()};
            // Act
            var result = _target.CreateGame("TheGame", DateTime.Today, nodes, nodes[0]);
            // Assert
            Check.That(result.Id).Not.IsEqualTo(0);
            Check.That(result.StartDate).IsEqualTo(DateTime.Today);
            Check.That(result.Nodes).ContainsExactly(nodes);
            Check.That(result.FirstNode).Equals(nodes[0]);
        }

        [Fact]
        public void CreateGameFirstNodeNotInNodes()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode(), new QuestionNode() };
            var firstNode = new TimerNode();
            // Act
            Check.ThatCode(() => _target.CreateGame("TheGame", DateTime.Today, nodes, firstNode)).Throws<ArgumentException>();
            // Assert
        }
    }
}
