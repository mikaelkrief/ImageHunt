using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NFluent;
using SQLitePCL;
using Xunit;

namespace ImageHuntTest.Services
{
    public class NodeServiceTest : ContextBasedTest
    {
        private NodeService _target;

        public NodeServiceTest()
        {
            _target = new NodeService(_context);
        }
        [Fact]
        public void AddNode()
        {
            // Arrange
            var node = new TimerNode {Delay = 10000};
            // Act
            _target.AddNode(node);
            // Assert
            Check.That(_context.Nodes).ContainsExactly(node);
        }

        [Fact]
        public void AddChildrenToNode()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode()};
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            var childrenNode = new TimerNode();
            // Act
            _target.AddChildren(nodes[1].Id, childrenNode);
            // Assert
            Check.That(nodes[1].Children).ContainsExactly(childrenNode);
            //Check.That(nodes[1].ChildrenRelation.First().Id).Not.IsEqualTo(0);
        }

        [Fact]
        public void AddChildrenToNodeUsingNode()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode() };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            var childrenNode = new TimerNode();
            // Act
            _target.AddChildren(nodes[1], childrenNode);
            // Assert
            Check.That(nodes[1].Children).ContainsExactly(childrenNode);
            //Check.That(nodes[1].ChildrenRelation.First().Id).Not.IsEqualTo(0);
        }
        [Fact]
        public void GetNodeFromId()
        {
            // Arrange
            var nodes = new List<Node>(){new TimerNode(){Id = 1}, new TimerNode(){Id = 2}, new TimerNode(){Id = 3}};
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var resultNode = _target.GetNode(2);
            // Assert
            Check.That(resultNode).IsEqualTo(nodes.Single(n => n.Id == 2));
        }

    }
}
