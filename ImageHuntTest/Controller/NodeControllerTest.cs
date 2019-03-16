using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHunt.Updater;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

        public NodeControllerTest()
      {
            _testContainerBuilder.RegisterInstance(_nodeService = A.Fake<INodeService>());
          _testContainerBuilder.RegisterInstance(_gameService = A.Fake<IGameService>());
          _testContainerBuilder.RegisterInstance(_teamService = A.Fake<ITeamService>());
            _testContainerBuilder.RegisterInstance(_mapper = AutoMapper.Mapper.Instance);
          _testContainerBuilder.RegisterInstance(_updater = A.Fake<IUpdater>()).Named<IUpdater>("UpdateNodePoints");
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
        _target.AddRelationToNode(relationRequest);
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
            _target.RemoveRelationToNode(relationRequest);
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
      _target.AddRelationsWithAnswers(relationsRequest);
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
        _target.RemoveRelationToNode(relationRequest);
        // Assert
        A.CallTo(() => _nodeService.RemoveChildren(1, A<int>._)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => _nodeService.UnlinkAnswerToNode(1)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Fact]
      public void RemoveNode()
      {
        // Arrange
        
        // Act
        var result = _target.RemoveNode(2);
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
        var result = _target.RemoveRelation(1, 2);
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
            var result = _target.GetNodeById(1);
            // Assert
            A.CallTo(() => _nodeService.GetNode(1)).MustHaveHappened();
        }

        [Fact]
        public void UpdateNode()
        {
            // Arrange
            var nodeRequest = new NodeUpdateRequest() {Id = 1, Name = "toto"};
            // Act
            _target.UpdateNode(nodeRequest);
            // Assert
            A.CallTo(() => _nodeService.GetNode(nodeRequest.Id)).MustHaveHappened();
            A.CallTo(() => _nodeService.UpdateNode(A<Node>._)).MustHaveHappened();
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
            var result = _target.GetNodesByType(NodeTypes.Picture.ToString(), 1);
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
            var result = _target.GetNextNodeForTeam(1);
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
            var result = _target.BatchUpdateNode(batchUpdateNodeRequest);
            // Assert
            A.CallTo(() => _updater.Execute()).MustHaveHappened();
        }
  }
}
