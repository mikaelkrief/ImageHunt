using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Model;
using ImageHuntCore.Model;
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
            orgNode.Latitude = 15.3;
            orgNode.Longitude = 6.22;
            orgNode.Image = new Picture(){Id = 15, Image = new byte[15]};
            orgNode.Points = 16;
            // Act
            var newNode = NodeFactory.UpdateNode(orgNode, NodeResponse.TimerNodeType);
            // Assert
            Check.That(newNode).Not.Equals(orgNode);
            Check.That(newNode.Name).Equals(orgNode.Name);
            Check.That(newNode.Latitude).Equals(orgNode.Latitude);
            Check.That(newNode.Longitude).Equals(orgNode.Longitude);
            Check.That(newNode.Image).Equals(orgNode.Image);
            Check.That(newNode.Points).Equals(orgNode.Points);
        }
    }
}
