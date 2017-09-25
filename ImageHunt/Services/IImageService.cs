using ImageHunt.Model;

namespace ImageHunt.Services
{
    public interface IImageService : IService
    {
        void AddPicture(Picture picture);
        Picture GetPictureById(int pictureId);
      (double, double) ExtractLocationFromImage(Picture picture);
    }
}