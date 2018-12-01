using System.IO;
using System.Threading.Tasks;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IImageWebService
    {
        Task<int> UploadImage(Stream imageStream);
    }
}