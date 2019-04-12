using System.IO;
using System.Threading.Tasks;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public interface IImageService : IService
    {
        Task AddPicture(Picture picture);
        Task<Picture> GetPictureById(int pictureId);
      (double, double) ExtractLocationFromImage(Picture picture);
      Picture GetImageForNode(Node node, bool includePictureBytes = false);
      Picture GetPictureFromStream(Stream fileStream);
    }
}
