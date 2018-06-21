using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public interface IImageService : IService
    {
        void AddPicture(Picture picture);
        Task<Picture> GetPictureById(int pictureId);
      (double, double) ExtractLocationFromImage(Picture picture);
    }
}
