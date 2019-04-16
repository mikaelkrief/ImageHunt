using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Services;
using ImageHunt.Updater;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
  [Collection("AutomapperFixture")]
  public class NodeControllerTest : BaseTest<NodeController>
    {
      private INodeService _nodeService;
        private IGameService _gameService;
        private ITeamService _teamService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IUpdater _updater;
        private IImageService _imageService;

        public NodeControllerTest()
      {
            TestContainerBuilder.RegisterInstance(_nodeService = A.Fake<INodeService>());
          TestContainerBuilder.RegisterInstance(_gameService = A.Fake<IGameService>());
          TestContainerBuilder.RegisterInstance(_teamService = A.Fake<ITeamService>());
          TestContainerBuilder.RegisterInstance(_imageService = A.Fake<IImageService>());
            TestContainerBuilder.RegisterInstance(_mapper = AutoMapper.Mapper.Instance);
          TestContainerBuilder.RegisterInstance(_updater = A.Fake<IUpdater>()).Named<IUpdater>("UpdateNodePoints");
            Build();
      }

      [Fact]
      public void AddRelationToNode()
      {
        // Arrange
        var relationRequest = new NodeRelationRequest()
        {
          NodeId = 1,
          ChildrenId = 2
        };
        // Act
        Target.AddRelationToNode(relationRequest);
        // Assert
        A.CallTo(() => _nodeService.AddChildren(1, A<int>._)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => _nodeService.LinkAnswerToNode(3, 2)).MustNotHaveHappened();
      }

    [Fact]
        public void RemoveRelationToNode()
        {
            // Arrange
            var relationRequest = new NodeRelationRequest()
            {
                NodeId = 1,
                ChildrenId = 2
            };
            // Act
            Target.RemoveRelationToNode(relationRequest);
            // Assert
            A.CallTo(() => _nodeService.RemoveChildren(1, A<int>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

      [Fact]
      public void AddRelationsWithAnswer()
      {
        // Arrange
        var relationsRequest = new List<NodeRelationRequest>()
        {
          new NodeRelationRequest(){
            NodeId = 1,
            ChildrenId = 2,
            AnswerId = 3
          },
          new NodeRelationRequest(){
            NodeId = 1,
            ChildrenId = 3,
            AnswerId = 2
          },
        };
        A.CallTo(() => _nodeService.GetNode(1)).Returns(new ChoiceNode(){Children = { new TimerNode(), new ChoiceNode(), new FirstNode()}});

      // Act
      Target.AddRelationsWithAnswers(relationsRequest);
      // Assert
        A.CallTo(() => _nodeService.GetNode(1)).MustHaveHappened();
        A.CallTo(() => _nodeService.RemoveAllChildren(A<Node>._)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => _nodeService.AddChildren(A<int>._, A<int>._)).MustHaveHappened(Repeated.Exactly.Twice);
        A.CallTo(() => _nodeService.LinkAnswerToNode(A<int>._, A<int>._)).MustHaveHappened(Repeated.Exactly.Twice);
      }

      [Fact]
      public void RemoveRelationWithAnswer()
      {
        // Arrange
        var relationRequest = new NodeRelationRequest()
        {
          NodeId = 1,
          ChildrenId = 2,
          AnswerId = 1
        };
        // Act
        Target.RemoveRelationToNode(relationRequest);
        // Assert
        A.CallTo(() => _nodeService.RemoveChildren(1, A<int>._)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => _nodeService.UnlinkAnswerToNode(1)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Fact]
      public void RemoveNode()
      {
        // Arrange
        
        // Act
        var result = Target.RemoveNode(2);
        // Assert
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(()=>_nodeService.GetNode(2)).MustHaveHappened();
        A.CallTo(()=>_nodeService.RemoveNode(A<Node>._)).MustHaveHappened();
      }

      [Fact]
      public void RemoveRelation()
      {
        // Arrange
        
        // Act
        var result = Target.RemoveRelation(1, 2);
        // Assert
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _nodeService.GetNode(A<int>._)).MustHaveHappened(Repeated.Exactly.Twice);
        A.CallTo(() => _nodeService.RemoveRelation(A<Node>._, A<Node>._)).MustHaveHappened();
      }

        [Fact]
        public void GetNodeById()
        {
            // Arrange
            
            // Act
            var result = Target.GetNodeById(1);
            // Assert
            A.CallTo(() => _nodeService.GetNode(1)).MustHaveHappened();
        }

        [Fact]
        public async Task UpdateNode_ObjectNode()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            var nodeRequest = new NodeUpdateRequest()
            {
                Id = 1, Name = "toto",
                NodeType = NodeResponse.ObjectNodeType,
                Points = 16,
                Latitude = 61,
                Longitude = 1,
                Action = "sdffsdf"
            };
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            var expectedNode = new ObjectNode(){Action = nodeRequest.Action};
            A.CallTo(() => _nodeService.UpdateNode(A<Node>.That.Matches(n=>MatchNode(n, expectedNode)))).MustHaveHappened();
        }
        [Fact]
        public async Task UpdateNode_TimerNode()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            var nodeRequest = new NodeUpdateRequest()
            {
                Id = 1, Name = "toto",
                NodeType = NodeResponse.TimerNodeType,
                Points = 16,
                Latitude = 61,
                Longitude = 1,
                Delay = 15,
            };
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            var expectedNode = new TimerNode(){Delay = nodeRequest.Delay.Value};
            A.CallTo(() => _nodeService.UpdateNode(A<Node>.That.Matches(n=>MatchNode(n, expectedNode)))).MustHaveHappened();
        }

        private bool MatchNode(Node node, Node expectedNode)
        {
            Check.That(node.NodeType).Equals(expectedNode.NodeType);
            switch (node.NodeType)
            {
                case NodeResponse.ObjectNodeType:
                    var objectNode = node as ObjectNode;
                    var expectedObjectNode = expectedNode as ObjectNode;
                    Check.That(objectNode.Action).Equals(objectNode.Action);
                    break;
                case NodeResponse.HiddenNodeType:
                    var hiddenNode = node as HiddenNode;
                    var expectedHiddenNode = expectedNode as HiddenNode;
                    Check.That(hiddenNode.LocationHint).Equals(expectedHiddenNode.LocationHint);
                    break;
                case NodeResponse.BonusNodeType:
                    var bonusNode = node as BonusNode;
                    var expectedBonusNode = expectedNode as BonusNode;
                    Check.That(bonusNode.Location).Equals(expectedBonusNode.Location);
                    Check.That(bonusNode.BonusType).Equals(expectedBonusNode.BonusType);
                    break;
                case NodeResponse.TimerNodeType:
                    var timerNode = node as TimerNode;
                    var expectedTimerNode = expectedNode as TimerNode;
                    Check.That(timerNode.Delay).Equals(expectedTimerNode.Delay);
                    break;
            }
            return true;
        }

        [Fact]
        public async Task UpdateNode_HiddenNode()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            var nodeRequest = new NodeUpdateRequest()
            {
                Id = 1, Name = "toto",
                NodeType = NodeResponse.HiddenNodeType,
                Points = 16,
                Latitude = 61,
                Longitude = 1,
                Hint = "sdffsdf"
            };
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            var expectedNode = new HiddenNode() { LocationHint = nodeRequest.Hint };

            A.CallTo(() => _nodeService.UpdateNode(A<Node>.That.Matches(n=>MatchNode(n, expectedNode)))).MustHaveHappened();

        }
        [Fact]
        public async Task UpdateNode_BonusNode()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            var nodeRequest = new NodeUpdateRequest()
            {
                Id = 1, Name = "toto",
                NodeType = NodeResponse.BonusNodeType,
                Points = 16,
                Latitude = 61,
                Longitude = 1,
                Hint = "sdffsdf",
                Bonus = 1,
            };
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            var expectedNode = new BonusNode() { Location = nodeRequest.Hint, BonusType = (BonusNode.BONUSTYPE) nodeRequest.Bonus };

            A.CallTo(() => _nodeService.UpdateNode(A<Node>.That.Matches(n=>MatchNode(n, expectedNode)))).MustHaveHappened();

        }

        [Fact]
        public async Task UpdateNode_With_Change_Type()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            var nodeRequest = new NodeUpdateRequest() {Id = 1, Name = "toto", NodeType = NodeResponse.BonusNodeType};
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            A.CallTo(() => _nodeService.UpdateNode(A<Node>._)).MustHaveHappened();
            A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._)).MustHaveHappened();
        }
        [Fact]
        public async Task UpdateNode_With_Image()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode(){Image = new Picture(){Id = 15}});
            A.CallTo(() => _imageService.GetPictureById(A<int>._)).Returns(new Picture() {Id = 16});
            var nodeRequest = new NodeUpdateRequest() {Id = 1, Name = "toto", NodeType = NodeResponse.BonusNodeType, ImageId = 16};
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            A.CallTo(() => _nodeService.UpdateNode(A<Node>._)).MustHaveHappened();
            A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._)).MustHaveHappened();
        }
        [Fact]
        public async Task UpdateNode_With_Image_Org_Node_Had_No_Image()
        {
            // Arrange
            A.CallTo(() => _nodeService.GetNode(A<int>._)).Returns(new ObjectNode());
            A.CallTo(() => _imageService.GetPictureById(A<int>._)).Returns(new Picture() {Id = 16});
            var nodeRequest = new NodeUpdateRequest() {Id = 1, Name = "toto", NodeType = NodeResponse.BonusNodeType, ImageId = 16};
            // Act
            await Target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            A.CallTo(() => _nodeService.UpdateNode(A<Node>._)).MustHaveHappened();
            A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._)).MustHaveHappened();
        }

        [Fact]
        public void GetNodeByType()
        {
            // Arrange
            var nodes = new List<Node>()
            {
                new FirstNode(),
                new ObjectNode(),
                new PictureNode(),
                new ChoiceNode(),
                new LastNode()
            };
            A.CallTo(() => _gameService.GetNodes(A<int>._, NodeTypes.Picture)).Returns(nodes.Where(n=>n.NodeType == NodeResponse.PictureNodeType));
            // Act
            var result = Target.GetNodesByType(NodeTypes.Picture.ToString(), 1);
            // Assert
            A.CallTo(() => _gameService.GetNodes(1, NodeTypes.Picture)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            var resultNodes = (result as OkObjectResult).Value as IEnumerable<NodeResponse>;
            Check.That(resultNodes.Extracting("NodeType")).ContainsExactly(NodeResponse.PictureNodeType);
        }
        [Fact]
        public void GetNextNodeForTeam()
        {
            // Arrange
            var currentNode = new TimerNode();
            var team = new Team(){CurrentNode = currentNode};
            A.CallTo(() => _teamService.GetTeamById(1)).Returns(team);
            // Act
            var result = Target.GetNextNodeForTeam(1);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _teamService.GetTeamById(1)).MustHaveHappened();
            A.CallTo(() => _nodeService.GetNode(A<int>._)).MustHaveHappened();
        }

        [Fact]
        public void Should_BatchUpdateNode_Succeed()
        {
            // Arrange
            BatchUpdateNodeRequest batchUpdateNodeRequest = new BatchUpdateNodeRequest()
            {
                GameId = 15,
                UpdaterArgument = @"\d*_(?'seed'\d)\.jpg",
                UpdaterType = "UpdateNodePoints"
            };

            // Act
            var result = Target.BatchUpdateNode(batchUpdateNodeRequest);
            // Assert
            A.CallTo(() => _updater.Execute()).MustHaveHappened();
        }
  }
}
