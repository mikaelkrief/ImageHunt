using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Services;
using ImageHuntCore;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class ImageServiceTest : ContextBasedTest<HuntContext>
    {
        private ImageService _service;
        private ILogger<ImageService> _logger;
        private IBlobProvider _blobProvider;

        public ImageServiceTest()
        {
            _logger = A.Fake<ILogger<ImageService>>();
            _blobProvider = A.Fake<IBlobProvider>();
            _service = new ImageService(_context, _logger, _blobProvider);
        }

        [Fact]
        public async Task AddPicture()
        {
            // Arrange

            var picture = new Picture() {Image = new byte[] {1, 5, 6}};
            // Act
            await _service.AddPicture(picture);
            // Assert
            Check.That(_context.Pictures).ContainsExactly(picture);
            A.CallTo(() => _blobProvider.UploadFromByteArrayAsync(A<byte[]>._)).MustHaveHappened();
        }

        [Fact]
        public async Task GetPictureById_Not_In_cloud()
        {
            // Arrange
            var pictures = new List<Picture>() {new Picture(), new Picture(), new Picture()};
            _context.Pictures.AddRange(pictures);
            _context.SaveChanges();
            // Act
            var result = await _service.GetPictureById(pictures[1].Id);
            // Assert
            Check.That(result).Equals(pictures[1]);
            A.CallTo(() => _blobProvider.DownloadToByteArrayAsync(A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task GetPictureById_In_cloud()
        {
            // Arrange
            var pictures = new List<Picture>() {
                new Picture(){CloudUrl = "http://test"},
                new Picture(){CloudUrl = "http://test"},
                new Picture(){CloudUrl = "http://test" },

            };
            _context.Pictures.AddRange(pictures);
            _context.SaveChanges();
            // Act
            var result = await _service.GetPictureById(pictures[1].Id);
            // Assert
            Check.That(result).Equals(pictures[1]);
            A.CallTo(() => _blobProvider.DownloadToByteArrayAsync(A<string>._)).MustHaveHappened();
        }

        [Fact]
        public void ExtractLocationFromImage()
        {
            // Arrange
            var picture = new Picture()
            {
                Image = GetImageFromResource(Assembly.GetExecutingAssembly(),
                    "ImageHuntTest.TestData.IMG_20170920_180905.jpg")
            };
            // Act
            var result = _service.ExtractLocationFromImage(picture);
            // Assert
            Check.That(Math.Abs(result.Item1 - 59.3278160094444)).IsStrictlyLessThan(0.001);
            Check.That(Math.Abs(result.Item2 - 18.0551338194444)).IsStrictlyLessThan(0.001);
        }

        [Fact]
        public void ExtractLocationFromImageWithoutGPSLocation()
        {
            // Arrange
            var picture = new Picture()
            {
                Image = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.image1.jpg")
            };
            // Act
            var result = _service.ExtractLocationFromImage(picture);
            // Assert
            Check.That(result.Item1).Equals(double.NaN);
            Check.That(result.Item2).Equals(double.NaN);
        }

        [Fact]
        public void GetImageForNode()
        {
            // Arrange
            var pictures = new List<Picture>() {new Picture(), new Picture() {Image = new byte[10]}, new Picture()};
            _context.Pictures.AddRange(pictures);
            var nodes = new List<Node>()
            {
                new PictureNode() {Image = pictures[1]},
                new PictureNode() {Image = pictures[0]}
            };
            _context.Nodes.AddRange(nodes);
            _context.SaveChanges();
            // Act
            var picture = _service.GetImageForNode(nodes[0]);
            // Assert
            Check.That(picture).Equals(pictures[1]);
            Check.That(picture.Image).IsNull();
        }
    }
}