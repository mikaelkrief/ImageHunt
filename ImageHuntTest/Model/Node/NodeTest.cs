using ImageHunt.Computation;
using ImageHunt.Model;
using ImageHunt.Model.Node;
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
    }
}
