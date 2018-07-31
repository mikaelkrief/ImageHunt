using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
  [Collection("AutomapperFixture")]
  public class NodeControllerTest
    {
      private NodeController _target;
      private INodeService _nodeService;

      public NodeControllerTest()
      {
        _nodeService = A.Fake<INodeService>();
        _target = new NodeController(_nodeService);
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
        A.CallTo(() => _nodeService.GetNode(1)).Returns(new QuestionNode(){Children = { new TimerNode(), new QuestionNode(), new FirstNode()}});

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
  }
}
