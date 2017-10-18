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
            
            // Act
            _target.AddChildren(nodes[1].Id, nodes[2].Id);
            // Assert
            Check.That(nodes[1].Children).ContainsExactly(nodes[2]);
            //Check.That(nodes[1].ChildrenRelation.First().Id).Not.IsEqualTo(0);
        }
        [Fact]
        public void RemoveChildrenToNode()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode(), new ObjectNode(), new PictureNode()};
            nodes[1].ChildrenRelation.Add(new ParentChildren(){Parent = nodes[1], Children = nodes[3] }); 
            nodes[1].ChildrenRelation.Add(new ParentChildren(){Parent = nodes[1], Children = nodes[4] }); 
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            
            // Act
            _target.RemoveChildren(nodes[1].Id, nodes[1].Children[0].Id);
            // Assert
            Check.That(nodes[1].Children).ContainsExactly(nodes[4]);
            //Check.That(nodes[1].ChildrenRelation.First().Id).Not.IsEqualTo(0);
        }

        [Fact]
        public void AddChildrenToNodeUsingNode()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode() };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            _target.AddChildren(nodes[1], nodes[2]);
            // Assert
            Check.That(nodes[1].Children).ContainsExactly(nodes[2]);
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
      [Fact]
      public void FindImageByLocation()
      {
        // Arrange
        var pictureNodes = new List<Node>()
        {
          new PictureNode(){Latitude = 15, Longitude = 15},
          new PictureNode(){Latitude = 16, Longitude = 16},
          new PictureNode(){Latitude = 17, Longitude = 17},
          new PictureNode(){Latitude = 18, Longitude = 18},
          new PictureNode(){Latitude = 59.327816, Longitude = 18.055133},


        };
        var games = new List<Game>() { new Game(), new Game() { Nodes = pictureNodes } };
        _context.Games.AddRange(games);
        _context.SaveChanges();
        var picture = new Picture() { Image = GetImageFromResource("ImageHuntTest.TestData.IMG_20170920_180905.jpg") };
        // Act
        var result = _target.FindPictureNodeByLocation(games[1].Id, picture);
        // Assert
        Check.That(result).Equals(pictureNodes[4]);
      }

  }
}
