using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
    public interface IImageService : IService
    {
        void AddPicture(Picture picture);
        Picture GetPictureById(int pictureId);
    }
}
