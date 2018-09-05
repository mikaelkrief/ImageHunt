﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Services;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using TestUtilities;
using Xunit;

namespace ImageHuntWebServiceClientTest.WebServices
{
    public class PasscodeWebServiceTest : WebServiceBaseTest
    {
        private PasscodeWebService _target;

        public PasscodeWebServiceTest()
        {
            _target = new PasscodeWebService(_httpClient);
        }

        [Fact]
        public void RedeemPasscode()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };

            A.CallTo(_fakeHttpMessageHandler)
                .Where(x => x.Method.Name == "PatchAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(httpResponse);

            // Act
            _target.RedeemPasscode(1, 1, "toto");
            // Assert
        }
    }
}
