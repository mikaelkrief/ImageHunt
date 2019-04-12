using System.Threading.Tasks;

namespace ImageHuntCore
{
    public interface IBlobProvider
    {
        Task<string> UploadFromByteArrayAsync(byte[] bytes);
        Task<byte[]> DownloadToByteArrayAsync(string cloudUrl);
    }
}