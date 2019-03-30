using System.Collections.Generic;
using ImageHunt.Helpers;
using ImageHunt.Model;
using ImageHuntCore.Model.Node;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Helpers
{
    public class NodeHelperTest : BaseTest
    {
        [Fact]
        public void Should_HaveChildren_link_2_nodes()
        {
            // Arrange
            var parentNode = new ObjectNode();
            var childNode = new TimerNode();
            // Act
            parentNode.HaveChild(childNode);
            // Assert
            Check.That(parentNode.Children).Contains(childNode);
        }

        [Fact]
        public void Should_Create_Path_From_Old_Path_in_new()
        {
            // Arrange
            var orgNodes = new List<Node>()
            {
                new WaypointNode(){Id = 1},
                new WaypointNode(){Id = 2},
                new WaypointNode(){Id = 3},
                new WaypointNode(){Id = 4},
                new WaypointNode(){Id = 5},
                new WaypointNode(){Id = 6},
            };
            orgNodes[0].HaveChild(orgNodes[1]);
            orgNodes[1].HaveChild(orgNodes[2]);
            orgNodes[2].HaveChild(orgNodes[3]);
            orgNodes[3].HaveChild(orgNodes[4]);
            orgNodes[4].HaveChild(orgNodes[5]);
            var newNodes = new List<Node>();
            orgNodes.ForEach(n=>newNodes.Add(NodeFactory.DuplicateNode(n)));
            // Act
            newNodes[0].DuplicatePath(orgNodes, newNodes);
            // Assert
        }
    }
}
