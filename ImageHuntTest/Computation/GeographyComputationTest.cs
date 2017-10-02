using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt.Computation;
using NFluent;
using Xunit;

namespace ImageHuntTest.Computation
{
    public class GeographyComputationTest
    {
      [Theory]
      [InlineData(45, 0, 0.785398163397448, 0d)]
      [InlineData(90, 0, 1.5707963267949, 0d)]
      [InlineData(0, 85d, 0d, 1.48352986419518)]
      public void DegToRad(double degLat, double degLng, double radLatExp, double radLngExp)
      {
        // Arrange
        
        // Act
        var result = GeographyComputation.DegToRad((degLat, degLng));
        // Assert
        Check.That(result.Item1).IsEqualsWithDelta(radLatExp, 0.0001);
        Check.That(result.Item2).IsEqualsWithDelta(radLngExp, 0.0001);
      }

      [Theory]
      [InlineData(0.785398163397448, 0d, 45d, 0)]
      [InlineData(1.5707963267949, 0d, 90, 0)]
      public void RadToDeg(double radLat, double radLng, double degLatExp, double degLngExp)
      {
        // Arrange
        
        // Act
        var result = GeographyComputation.RadToDeg((radLat, radLng));
        // Assert
        Check.That(result.Item1).IsEqualsWithDelta(degLatExp, 0.0001);
        Check.That(result.Item2).IsEqualsWithDelta(degLngExp, 0.0001);
      }
      [Fact]
      public void CenterOfGeoPoints()
      {
        // Arrange
        var points = new List<(double, double)>()
        {
          (48.8501065, 2.327722),
          (48.851291, 2.3318698),
          (48.8537828, 2.3310879)
        };
        // Act
        var result = GeographyComputation.CenterOfGeoPoints(points);
        // Assert
        Check.That(result.Item1).IsEqualsWithDelta(48.8517267806692, 0.001);
        Check.That(result.Item2).IsEqualsWithDelta(2.33022653262665, 0.001);
      }

      [Fact]
      public void DistanceBetweenTwoPosition()
      {
        // Arrange
        var point1 = (48.8501065, 2.327722);
        var point2 = (48.851291, 2.3318698);
        // Act
        var distance = GeographyComputation.Distance(point1, point2);
        // Assert
        Check.That(distance).IsEqualsWithDelta(8600.78833324218, 0.001);
      }
    }
}
