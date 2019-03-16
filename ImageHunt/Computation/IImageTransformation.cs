namespace ImageHunt.Computation
{
  public interface IImageTransformation
  {
    byte[] Thumbnail(byte[] oriImage, int width, int height);
  }
}
