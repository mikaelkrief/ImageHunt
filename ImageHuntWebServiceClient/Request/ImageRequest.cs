using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHuntWebServiceClient.Request
{
    public class ImageRequest
    {
        public string PictureId { get; set; }
        public string ThumbnailWidth { get; set; }
        public string ThumbnailHeight { get; set; }
    }
}
