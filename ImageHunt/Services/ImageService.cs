using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
  public class ImageService : AbstractService, IImageService
  {
    public ImageService(HuntContext context, ILogger<ImageService> logger) : base(context, logger)
    {

    }

    public void AddPicture(Picture picture)
    {
      Context.Pictures.Add(picture);
      Context.SaveChanges();
    }

    public async Task<Picture> GetPictureById(int pictureId)
    {
      return Queryable.Single<Picture>(Context.Pictures, p => p.Id == pictureId);
    }

    public virtual (double, double) ExtractLocationFromImage(Picture picture)
    {
      using (var imageStream = new MemoryStream(picture.Image))
      {
        using (var image = new MagickImage(imageStream))
        {
          var exifProfile = image.GetExifProfile();
          if (exifProfile == null || exifProfile.Values.All(v => v.Tag != ExifTag.GPSLatitude))
            return (double.NaN, double.NaN);
          var gpsLatitude = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLatitude).Value as Rational[];
          var gpsLatitudeRef = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLatitudeRef).Value as string;
          var gpsLongitude = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLongitude).Value as Rational[];
          var gpsLongitudeRef = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLongitudeRef).Value as string;
          double latitude, longitude;
          int latSign = gpsLatitudeRef == "N" ? 1 : -1;
          int lngSign = gpsLongitudeRef == "E" ? 1 : -1;
          latitude = (double)gpsLatitude[0].Numerator / gpsLatitude[0].Denominator +
                     (double)gpsLatitude[1].Numerator / (gpsLatitude[1].Denominator * 60) +
                     (double)gpsLatitude[2].Numerator / (gpsLatitude[2].Denominator * 3600);
          latitude *= latSign;
          longitude = (double)gpsLongitude[0].Numerator / gpsLongitude[0].Denominator +
                      (double)gpsLongitude[1].Numerator / (gpsLongitude[1].Denominator * 60) +
                      (double)gpsLongitude[2].Numerator / (gpsLongitude[2].Denominator * 3600);
          longitude *= lngSign;
          return (latitude, longitude);
        }
      }
    }

    public Picture GetImageForNode(Node node, bool includePictureBytes = false)
    {
      var pictureNode = Context.PictureNodes.Include(p => p.Image).Single(p => p.Id == node.Id);
      if (!includePictureBytes)
        pictureNode.Image.Image = null;
      return pictureNode.Image;
    }
  }
}
