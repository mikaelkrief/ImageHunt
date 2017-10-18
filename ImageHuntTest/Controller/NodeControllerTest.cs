using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Request;
using ImageHunt.Services;
using Xunit;

namespace ImageHuntTest.Controller
{
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
      }

        [Fact]
        public void RemoveRelationToNode()
        {
            // Arrange
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
    }
}
