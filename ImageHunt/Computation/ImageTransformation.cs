using System.IO;
using ImageMagick;

namespace ImageHunt.Computation
{
    public class ImageTransformation : IImageTransformation
  {
        public byte[] Thumbnail(byte[] oriImage, int width, int height)
        {
            using (var imageStream = new MemoryStream(oriImage))
            {
                using (var image = new MagickImage(imageStream))
                {
                    image.Resize(width, height);
                    image.Strip();
                    using (var destImageStream = new MemoryStream())
                    {
                        image.Write(destImageStream);
                        destImageStream.Seek(0, SeekOrigin.Begin);
                        return destImageStream.ToArray();
                    }
                }

            }
        }
    }
}
