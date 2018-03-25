using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.AspNetCore.StaticFiles;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class ImageServiceTest : ContextBasedTest
    {
        private ImageService _service;

        public ImageServiceTest()
        {
            _service = new ImageService(_context);
        }
        [Fact]
        public void AddPicture()
        {
            // Arrange
            var picture = new Picture(){Image = new byte[]{1,5,6}};
            // Act
            _service.AddPicture(picture);
            // Assert
            Check.That(_context.Pictures).ContainsExactly(picture);
        }

        [Fact]
        public void GetPictureById()
        {
            // Arrange
            var pictures = new List<Picture>(){new Picture(), new Picture(), new Picture()};
            _context.Pictures.AddRange(pictures);
            _context.SaveChanges();
            // Act
            var result = _service.GetPictureById(pictures[1].Id);
            // Assert
            Check.That(result).Equals(pictures[1]);
        }

        [Fact]
        public void ExtractLocationFromImage()
        {
            // Arrange
            var picture = new Picture() { Image = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg") };
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
            var picture = new Picture() { Image = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.image1.jpg") };
            // Act
            var result = _service.ExtractLocationFromImage(picture);
            // Assert
            Check.That(result.Item1 ).Equals(double.NaN);
            Check.That(result.Item2 ).Equals(double.NaN);
        }

    }
}
