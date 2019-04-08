using ImageHunt.Helpers;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Helpers
{
    public class EntityHelperTest : BaseTest
    {
        [Fact]
        public void Should_Create_Code_succeed()
        {
            // Arrange
            
            // Act
            var result = EntityHelper.CreateCode(6);
            // Assert
            Check.That(result.Length).Equals(6);
        }
    }
}
