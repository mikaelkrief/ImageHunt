using AutoMapper;
using FakeItEasy;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class NodeServiceTest : ContextBasedTest
    {
        private NodeService _target;
        private ILogger<NodeService> _logger;

        public NodeServiceTest()
        {
            _logger = A.Fake<ILogger<NodeService>>();
            _target = new NodeService(_context, _logger);
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Node, Node>().ForSourceMember(x => x.Id, opt => opt.Ignore());
            });

        }
        [Fact]
        public void AddNode()
        {
            // Arrange
            var node = new TimerNode { Delay = 10000 };
            // Act
            _target.AddNode(node);
            // Assert
            Check.That(_context.Nodes).ContainsExactly(node);
        }

        [Fact]
        public void AddChildrenToNode()
        {
            // Arrange
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode() };
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
            var nodes = new List<Node>() { new TimerNode(), new TimerNode(), new TimerNode(), new ObjectNode(), new PictureNode() };
            nodes[1].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[1], Children = nodes[3] });
            nodes[1].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[1], Children = nodes[4] });
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
            var nodes = new List<Node>() { new TimerNode() { Id = 1 }, new TimerNode() { Id = 2 }, new TimerNode() { Id = 3 } };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var resultNode = _target.GetNode(2);
            // Assert
            Check.That(resultNode).IsEqualTo(nodes.Single(n => n.Id == 2));
        }

        [Fact]
        public void GetQuestionNodeFromId()
        {
            // Arrange
            var answers = new List<Answer>() { new Answer(), new Answer() };
            _context.Answers.AddRange(answers);
            _context.SaveChanges();
            var nodes = new List<Node>() { new TimerNode(), new QuestionNode() { Answers = answers }, new TimerNode() };
            foreach (var answer in _context.Answers)
            {
                answer.Node = nodes[1];
            }
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var resultNode = _target.GetNode(nodes[1].Id);

            // Assert
            Check.That(resultNode).IsInstanceOf<QuestionNode>();
            Check.That(((QuestionNode)resultNode).Answers).HasSize(2);
        }
        [Fact]
        public void GetPictureNodeFromId()
        {
            // Arrange
            var images = new List<Picture> { new Picture(), new Picture(), new Picture() };
            _context.Pictures.AddRange(images);
            _context.SaveChanges();
            var nodes = new List<Node>() { new TimerNode(),
                new PictureNode() { Image = images[1]},
                new TimerNode() };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var resultNode = _target.GetNode(nodes[1].Id);

            // Assert
            Check.That(((PictureNode) resultNode).Image).Equals(images[1]);
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
            var picture = new Picture() { Image = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg") };
            // Act
            var result = _target.FindPictureNodeByLocation(games[1].Id, (59.327816, 18.055133));
            // Assert
            Check.That(result).Equals(pictureNodes[4]);
        }

        [Fact]
        public void LinkAnswerToNode()
        {
            // Arrange
            var answers = new List<Answer>()
        {
          new Answer(){Id = 1},
          new Answer(){Id = 2},
          new Answer(){Id = 3},
        };
            _context.Answers.AddRange(answers);
            var nodes = new List<Node>()
      {
        new PictureNode(){Id = 1},
        new PictureNode(){Id = 2},
        new PictureNode(){Id = 3},
        new PictureNode(){Id = 4},
      };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            _target.LinkAnswerToNode(2, 3);
            // Assert
            Check.That(answers[0].Node).IsNull();
            Check.That(answers[1].Node).Equals(nodes[2]);
        }
        [Fact]
        public void LinkAnswerToNodeAnswerNotExist()
        {
            // Arrange
            var answers = new List<Answer>()
        {
          new Answer(){Id = 1},
          new Answer(){Id = 2},
          new Answer(){Id = 3},
        };
            _context.Answers.AddRange(answers);
            var nodes = new List<Node>()
      {
        new PictureNode(){Id = 1},
        new PictureNode(){Id = 2},
        new PictureNode(){Id = 3},
        new PictureNode(){Id = 4},
      };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            _target.LinkAnswerToNode(4, 3);
            // Assert
        }
        [Fact]
        public void LinkAnswerToNodeNodeNotExist()
        {
            // Arrange
            var answers = new List<Answer>()
        {
          new Answer(){Id = 1},
          new Answer(){Id = 2},
          new Answer(){Id = 3},
        };
            _context.Answers.AddRange(answers);
            var nodes = new List<Node>()
      {
        new PictureNode(){Id = 1},
        new PictureNode(){Id = 2},
        new PictureNode(){Id = 3},
        new PictureNode(){Id = 4},
      };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            _target.LinkAnswerToNode(2, 5);
            // Assert
            Check.That(answers[1].Node).IsNull();
        }
        [Fact]
        public void UnLinkAnswerToNode()
        {
            // Arrange
            var nodes = new List<Node>()
        {
          new PictureNode(){Id = 1},
          new PictureNode(){Id = 2},
          new PictureNode(){Id = 3},
          new PictureNode(){Id = 4},
        };
            var answers = new List<Answer>()
        {
          new Answer(){Id = 1, Node = nodes[2]},
          new Answer(){Id = 2},
          new Answer(){Id = 3, Node = nodes[3]},
        };
            _context.Nodes.AddRange(nodes);
            _context.Answers.AddRange(answers);
            _context.SaveChanges();
            // Act
            _target.UnlinkAnswerToNode(3);
            // Assert
            Check.That(answers[2].Node).IsNull();
        }
        [Fact]
        public void UnLinkAnswerToNodeAnswerNotExist()
        {
            // Arrange
            var nodes = new List<Node>()
        {
          new PictureNode(){Id = 1},
          new PictureNode(){Id = 2},
          new PictureNode(){Id = 3},
          new PictureNode(){Id = 4},
        };
            var answers = new List<Answer>()
        {
          new Answer(){Id = 1, Node = nodes[2]},
          new Answer(){Id = 2},
          new Answer(){Id = 3, Node = nodes[3]},
        };
            _context.Nodes.AddRange(nodes);
            _context.Answers.AddRange(answers);
            _context.SaveChanges();
            // Act
            Check.ThatCode(() => _target.UnlinkAnswerToNode(4)).DoesNotThrow();
            // Assert
        }

        [Fact]
        public void RemoveAllChildren()
        {
            // Arrange
            var nodes = new List<Node>()
        {
          new TimerNode(),
          new QuestionNode(),
          new PictureNode(),
          new PictureNode(),
          new PictureNode(),
        };
            ((QuestionNode)nodes[1]).ChildrenRelation = new List<ParentChildren>()
      {
        new ParentChildren(){Parent = nodes[1], Children = nodes[3]},
        new ParentChildren(){Parent = nodes[1], Children = nodes[4]},

      };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            Check.That(nodes[1].Children).HasSize(2);

            // Act
            _target.RemoveAllChildren(nodes[1]);
            // Assert
            Check.That(nodes[1].Children).HasSize(0);
        }

        [Fact]
        public void GetAnswer()
        {
            // Arrange
            var answers = new List<Answer> { new Answer(), new Answer(), new Answer() };
            _context.Answers.AddRange(answers);
            _context.SaveChanges();
            // Act
            var result = _target.GetAnswer(answers[1].Id);
            // Assert
            Check.That(result).Equals(answers[1]);
        }

        [Fact]
        public void RemoveNode()
        {
            // Arrange
            var nodes = new List<Node>
        {
          new FirstNode(),
          new QuestionNode(),
          new TimerNode(),
          new ObjectNode(),
          new LastNode()
        };
            _context.Nodes.AddRange(nodes);
            nodes[0].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[0], Children = nodes[1] } };
            nodes[1].ChildrenRelation = new List<ParentChildren>()
        {
          new ParentChildren(){Parent = nodes[1], Children = nodes[2]},
          new ParentChildren(){Parent = nodes[1], Children = nodes[3]},
          new ParentChildren(){Parent = nodes[1], Children = nodes[4]}
        };
            nodes[2].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[2], Children = nodes[4] } };
            nodes[3].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[3], Children = nodes[4] } };
            _context.SaveChanges();
            // Act
            _target.RemoveNode(nodes[1]);
            // Assert
            Check.That(_context.Nodes).Not.Contains(nodes[1]);

            Check.That(_context.ParentChildren).HasSize(2);
        }

        [Fact]
        public void RemoveQuestionNode()
        {
            // Arrange
            var answers = new List<Answer>() { new Answer(), new Answer(), new Answer() };
            var nodes = new List<Node>
        {
          new FirstNode(),
          new QuestionNode(){Answers = answers},
          new TimerNode(),
          new ObjectNode(),
          new LastNode(),
          new QuestionNode(){Answers = new List<Answer>(){new Answer()}}
        };
            _context.Nodes.AddRange(nodes);
            nodes[0].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[0], Children = nodes[1] } };
            nodes[2].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[2], Children = nodes[4] } };
            nodes[3].ChildrenRelation = new List<ParentChildren>() { new ParentChildren() { Parent = nodes[3], Children = nodes[4] } };
            _context.SaveChanges();
            _target.LinkAnswerToNode(answers[0].Id, nodes[2].Id);
            _target.LinkAnswerToNode(answers[1].Id, nodes[3].Id);
            _target.LinkAnswerToNode(answers[2].Id, nodes[4].Id);
            // Act
            _target.RemoveNode(nodes[1]);
            // Assert
            Check.That(_context.Nodes).Not.Contains(nodes[1]);

            Check.That(_context.ParentChildren).HasSize(2);
            Check.That(((QuestionNode)nodes[1]).Answers).HasSize(0);
            Check.That(_context.Answers).HasSize(1);
        }

        [Fact]
        public void RemoveRelation()
        {
            // Arrange
            var nodes = new List<Node> { new FirstNode(), new TimerNode(), new LastNode() };
            nodes[0].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[0], Children = nodes[1] });
            nodes[1].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[1], Children = nodes[2] });
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            _target.RemoveRelation(nodes[1], nodes[2]);
            // Assert
            Check.That(_context.ParentChildren).HasSize(1);
            Check.That(nodes[1].Children).HasSize(0);
        }

        [Fact]
        public void RemoveRelationQuestionNode()
        {
            // Arrange
            var answers = new List<Answer> { new Answer(), new Answer() };
            var nodes = new List<Node> { new FirstNode(), new QuestionNode() { Answers = answers }, new TimerNode(), new LastNode() };
            nodes[0].ChildrenRelation.Add(new ParentChildren() { Parent = nodes[0], Children = nodes[1] });
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            _target.AddChildren(nodes[1], nodes[2]);
            _target.AddChildren(nodes[1], nodes[3]);
            _target.LinkAnswerToNode(answers[0].Id, nodes[2].Id);
            _target.LinkAnswerToNode(answers[1].Id, nodes[3].Id);
            // Act
            _target.RemoveRelation(nodes[1], nodes[2]);
            // Assert
            Check.That(_context.ParentChildren).HasSize(2);
            Check.That(nodes[1].Children).HasSize(1);
            Check.That(_context.Answers).HasSize(1);
        }

        [Fact]
        public void UpdateNode()
        {
            // Arrange
            var nodes = new List<Node> {new FirstNode(), new PictureNode() {Name = "titi", Points = 9, Latitude = 45.6, Longitude = 8.3}, new TimerNode()};
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            var updatedNode = new PictureNode(){Id = nodes[1].Id, Name = "toto", Points = 15, Latitude = 65.3, Longitude = 4.3};
            // Act
            _target.UpdateNode(updatedNode);
            // Assert
            Check.That(nodes[1].Name).Equals(updatedNode.Name);
            Check.That(nodes[1].Points).Equals(updatedNode.Points);
            Check.That(nodes[1].Latitude).Equals(updatedNode.Latitude);
            Check.That(nodes[1].Longitude).Equals(updatedNode.Longitude);
        }
    }
}
