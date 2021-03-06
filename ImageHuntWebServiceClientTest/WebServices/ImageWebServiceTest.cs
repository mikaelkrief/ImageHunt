﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class ImageWebServiceTest : WebServiceBaseTest
    {
        private ILogger<IImageWebService> _logger;
        private ImageWebService _target;

        public ImageWebServiceTest()
        {
            _logger = A.Fake<ILogger<IImageWebService>>();
            _target = new ImageWebService(HttpClient, _logger);
        }

        [Fact]
        public async Task Should_Upload_Picture()
        {
            // Arrange
            var bytes = new byte[15];

            FakeResponse("ImageHuntWebServiceClientTest.Data.UploadImage_response.json");        
            using (var memStream = new MemoryStream(bytes))
            {

                // Act
                var result = await _target.UploadImage(memStream);
                // Assert
                A.CallTo(FakeHttpMessageHandler)
                    .Where(x => x.Method.Name == "SendAsync")
                    .WithReturnType<Task<HttpResponseMessage>>()
                    .MustHaveHappened();
            }
        }
    }
}
