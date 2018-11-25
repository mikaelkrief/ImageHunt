using System;
using ImageHunt.Model;
using ImageHuntCore.Model.Node;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model
{
  public class NodeFactoryTest
  {
    [Theory]
    [InlineData("TimerNode", typeof(TimerNode))]
    [InlineData("ObjectNode", typeof(ObjectNode))]
    [InlineData("QuestionNode", typeof(QuestionNode))]
    [InlineData("FirstNode", typeof(FirstNode))]
    [InlineData("LastNode", typeof(LastNode))]
    [InlineData("PictureNode", typeof(PictureNode))]
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