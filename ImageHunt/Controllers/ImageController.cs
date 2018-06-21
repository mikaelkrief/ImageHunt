using System;
using System.Net.Mime;
using System.Threading.Tasks;
using ImageHunt.Computation;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
      private readonly IImageTransformation _imageTransformation;

      public ImageController(IImageService imageService, IImageTransformation imageTransformation)
      {
        _imageService = imageService;
        _imageTransformation = imageTransformation;
      }
        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
          try
          {
            var picture = await _imageService.GetPictureById(imageId);
            return File(picture.Image, "image/jpeg");

          }
          catch (System.Exception e)
          {
            return new NotFoundObjectResult($"Image of id {imageId} not found");
          }
        }

      public async Task<IActionResult> GetThumbailById(int pictureId, int width, int height)
      {
        var picture = await _imageService.GetPictureById(pictureId);

        var thumbnail = _imageTransformation.Thumbnail(picture.Image, width, height);
        return File(thumbnail, "image/jpeg");
      }
    }
}
