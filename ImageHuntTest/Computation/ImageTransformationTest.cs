using System.IO;
using System.Reflection;
using ImageHunt.Computation;
using ImageMagick;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Computation
{
    public class ImageTransformationTest : BaseTest
    {
        private ImageTransformation _target;

        public ImageTransformationTest()
        {
            _target = new ImageTransformation();
        }

        [Fact]
        public void ThumbnailFromImage()
        {
            // Arrange
            var testImage = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");

            // Act
            var thumbnail = _target.Thumbnail(testImage, 150, 100);
            // Assert
            using (var stream = new MemoryStream(thumbnail))
            {
                using (var magick = new MagickImage(stream))
                {
                    Check.That(magick.Width).Equals(133);
                }
            }
        }
    }
}
