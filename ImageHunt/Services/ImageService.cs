using System.IO;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageMagick;

namespace ImageHunt.Services
{
  public class ImageService : AbstractService, IImageService
  {
    public ImageService(HuntContext context) : base(context)
    {

    }

    public void AddPicture(Picture picture)
    {
      Context.Pictures.Add(picture);
      Context.SaveChanges();
    }

    public Picture GetPictureById(int pictureId)
    {
      return Queryable.Single<Picture>(Context.Pictures, p => p.Id == pictureId);
    }

    public (double, double) ExtractLocationFromImage(Picture picture)
    {
      using (var imageStream = new MemoryStream(picture.Image))
      {
        using (var image = new MagickImage(imageStream))
        {
          var exifProfile = image.GetExifProfile();
          var gpsLatitude = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLatitude).Value as Rational[];
          var gpsLatitudeRef = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLatitudeRef).Value as string;
          var gpsLongitude = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLongitude).Value as Rational[];
          var gpsLongitudeRef = exifProfile.Values.First(v => v.Tag == ExifTag.GPSLongitudeRef).Value as string;
          double latitude, longitude;
          int latSign = gpsLatitudeRef == "N" ? 1 : -1;
          int lngSign = gpsLongitudeRef == "E" ? 1 : -1;
          latitude = gpsLatitude[0].Numerator / gpsLatitude[0].Denominator +
                     gpsLatitude[1].Numerator / (gpsLatitude[1].Denominator * 60) +
                     gpsLatitude[2].Numerator / (gpsLatitude[2].Denominator * 3600);
          latitude *= latSign;
          longitude = gpsLongitude[0].Numerator / gpsLongitude[0].Denominator +
                      gpsLongitude[1].Numerator / (gpsLongitude[1].Denominator * 60) +
                      gpsLongitude[2].Numerator / (gpsLongitude[2].Denominator * 3600);
          longitude *= lngSign;
          return (latitude, longitude);
        }
      }
    }
  }
}
