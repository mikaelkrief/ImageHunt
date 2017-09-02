using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Computation;
using Newtonsoft.Json.Schema;
using NFluent;
using Xunit;

namespace ScavengerHuntTests
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
            var node = new TimerNode("Toto", coordinate, 100);

            // Assert
            Check.That(node).IsNotNull();
        }
    }
}
