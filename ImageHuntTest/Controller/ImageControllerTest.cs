using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Computation;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class ImageControllerTest : BaseTest
    {
        private IImageService _imageService;
        private ImageController _target;
        private IImageTransformation _imageTransformation;

        public ImageControllerTest()
        {
            _imageService = A.Fake<IImageService>();
            _imageTransformation = A.Fake<IImageTransformation>();
            _target = new ImageController(_imageService, _imageTransformation);
        }
        [Fact]
        public async Task GetImageById()
        {
            // Arrange
            var testImage = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            A.CallTo(() => _imageService.GetPictureById(A<int>._))
                .Returns(new Picture(){Id = 1, Image = testImage});
            // Act
            var result = await _target.GetImageById(1) as FileContentResult;
            // Assert
            Check.That(result.FileContents).Equals(testImage);
        }
        [Fact]
        public async Task GetImageById_ImageNotFound()
        {
            // Arrange
            A.CallTo(() => _imageService.GetPictureById(A<int>._))
                .Throws<Exception>();
            // Act
            var result = await _target.GetImageById(1) ;
            // Assert
            Check.That(result).IsInstanceOf<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetThumbnailImageById()
        {
            // Arrange
            var testImage = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            A.CallTo(() => _imageService.GetPictureById(A<int>._))
                .Returns(new Picture() { Id = 1, Image = testImage });
            // Act
            var result = await _target.GetThumbailById(1, 150, 100) as FileContentResult;
            // Assert
            Check.That(result.FileContents).IsNotNull();
        }

        [Fact]
        public async Task GetImageAndthumbnailById()
        {
            // Arrange
            var testImage = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            A.CallTo(() => _imageService.GetPictureById(A<int>._))
                .Returns(new Picture() { Id = 1, Image = testImage });
            // Act
            var result = await _target.GetImageAndthumbnailById(1, 150, 100) as OkObjectResult;
            // Assert
            Check.That(result.Value).IsInstanceOf<FileContentResult[]>();
        }

        [Fact]
        public void UploadImage()
        {
            // Arrange
            var file = A.Fake<IFormFile>();

            // Act
            var result = _target.UploadImage(file);
            // Assert
            Check.That(result).IsInstanceOf<CreatedAtActionResult>();
            A.CallTo(() => _imageService.AddPicture(A<Picture>._)).MustHaveHappened();
        }
        [Fact]
        public void UploadImage_BadRequest()
        {
            // Arrange

            // Act
            var result = _target.UploadImage(null);
            // Assert
            Check.That(result).IsInstanceOf<BadRequestResult>();
        }
    }
}
