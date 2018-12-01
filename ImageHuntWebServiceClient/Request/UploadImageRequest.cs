using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ImageHuntWebServiceClient.Request
{
    public class UploadImageRequest

    {
        public int GameId { get; set; }
        public int TeamId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile FormFile { get; set; }
        public string ImageName { get; set; }
        public int? PictureId { get; set; }
    }
}