using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Helpers;
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
    }
}
