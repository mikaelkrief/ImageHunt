using System;
using ImageHunt.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model
{
  public class NodeFactoryTest
  {
    [Theory]
    [InlineData(NodeResponse.TimerNodeType, typeof(TimerNode))]
    [InlineData(NodeResponse.ObjectNodeType, typeof(ObjectNode))]
    [InlineData(NodeResponse.ChoiceNodeType, typeof(ChoiceNode))]
    [InlineData(NodeResponse.FirstNodeType, typeof(FirstNode))]
    [InlineData(NodeResponse.LastNodeType, typeof(LastNode))]
    [InlineData(NodeResponse.PictureNodeType, typeof(PictureNode))]
    public void NodeFromNodeType(string nodeName, Type expectedType)
    {
      // Arrange
      var nodeType = nodeName;
      // Act
      var node = NodeFactory.CreateNode(nodeType);
      // Assert
      Check.That(node.GetType()).Equals(expectedType);
    }
  }
}