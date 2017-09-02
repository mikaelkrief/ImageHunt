using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Computation;
using NFluent;
using Xunit;

namespace ScavengerHuntTests
{
    public class GeographyTest
    {
        [Fact]
        public void ComputeDistance()
        {
            // Arrange
            var geo1 = new Geography(48.0, 2.28, 10);
            geo1.DegToRad();
            var geo2 = new Geography(49.0, 2.28, 10);
            geo2.DegToRad();
            // Act
            var distance = geo1.Distance(geo2);
            // Assert
            Check.That(distance - 111231.361691082).IsStrictlyLessThan(0.001);
        }

        [Fact]
        public void ConvertDegreeToRadian()
        {
            // Arrange
            var geo = new Geography(45.0, 90.0, 10.0);
            // Act
            geo.DegToRad();
            // Assert
            Check.That(geo.Longitude - 0.785398163397448).IsStrictlyLessThan(0.001);
            Check.That(geo.Latitude - 1.5707963267949).IsStrictlyLessThan(0.001);
        }
    }
}
