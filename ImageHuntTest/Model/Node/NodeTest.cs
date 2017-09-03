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
            var coordinate = new Geography(10.3, 1.2, 10.0);
            coordinate.DegToRad();
            // Act
            var node = new TimerNode();
            node.Coordinate = coordinate;

            // Assert
            Check.That(node).IsNotNull();
        }
    }
}
