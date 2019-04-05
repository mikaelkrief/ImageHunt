using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Model;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest
{
    public class NodeFactoryTest : BaseTest
    {
        public NodeFactoryTest()
        {
            
        }

        [Fact]
        public void Should_UpdateNode_Change_NodeType()
        {
            // Arrange
            var orgNode = NodeFactory.CreateNode(NodeResponse.ObjectNodeType);
            orgNode.Name = "Toto";
            // Act
            var newNode = NodeFactory.UpdateNode(orgNode, NodeResponse.TimerNodeType);
            // Assert
            Check.That(newNode).Not.Equals(orgNode);
            Check.That(newNode.Name).Equals(orgNode.Name);
        }
    }
}
