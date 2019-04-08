using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model.Node
{
    public class NodeTest
    {
        public NodeTest()
        {
        }
        [Fact]
        public void Construct()
        {
            // Arrange

            // Act
            var node = new TimerNode();

            // Assert
            Check.That(node).IsNotNull();
        }

        [Fact]
        public void NodeType()
        {
            // Arrange
            var timerNode = new TimerNode();
            var questionNode = new ChoiceNode();
            // Act

            // Assert
            Check.That(timerNode.NodeType).Equals(NodeResponse.TimerNodeType);
            Check.That(questionNode.NodeType).Equals(NodeResponse.ChoiceNodeType);
        }
    }
}
