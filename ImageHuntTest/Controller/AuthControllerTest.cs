using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class AuthControllerTest
    {
        private AuthController _target;
        private HttpMessageHandler _fakeTokenMessageHandler;
        private readonly HttpClient _httpToken;
        private HttpMessageHandler _fakeUserMessageHandler;
        private readonly HttpClient _httpUser;
        private IAuthService _authService;
        private readonly IConfigurationRoot _configuration;

        public AuthControllerTest()
        {
            _fakeTokenMessageHandler = A.Fake<HttpMessageHandler>();
            _httpToken = new HttpClient(_fakeTokenMessageHandler) { BaseAddress = new Uri("http://test.com") };
            _fakeUserMessageHandler = A.Fake<HttpMessageHandler>();
            _httpUser = new HttpClient(_fakeUserMessageHandler) { BaseAddress = new Uri("http://test.com") };
            _authService = A.Fake<IAuthService>();
            _configuration = A.Fake<IConfigurationRoot>();
            var parameters = new AuthControllerParameters(_configuration, _httpToken, _httpUser, _authService);
            _target = new AuthController(parameters);
        }

        [Fact]
        public async Task GoogleSignIn()
        {
            // Arrange
            dynamic bearer = new JObject();
            bearer.code = "toto";
            bearer.redirectUri = "http://localhost";
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{\"access_token\":\"1545151515\", \"expires_in\":\"3521\", \"redirectUri\":\"http://localhost\"}");
            A.CallTo(_fakeTokenMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);
            response = new HttpResponseMessage();
            response.Content = new StringContent("{\"email\":\"toto@titi.com\", \"id\":\"3521\", \"redirectUri\":\"http://localhost\"}");
            A.CallTo(_fakeUserMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);

            // Act
            await _target.GoogleSignIn(bearer);
            // Assert
            A.CallTo(_fakeTokenMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .MustHaveHappened();
            A.CallTo(() => _authService.RefreshToken("toto@titi.com", "1545151515", A<DateTime>._)).MustHaveHappened();

        }
        [Fact]
        public async Task GoogleSignIn_UserNotExist()
        {
            // Arrange
            dynamic bearer = new JObject();
            bearer.code = "toto";
            bearer.redirectUri = "http://localhost";
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{\"access_token\":\"1545151515\", \"expires_in\":\"3521\", \"redirectUri\":\"http://localhost\"}");
            A.CallTo(_fakeTokenMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);
            response = new HttpResponseMessage();
            response.Content = new StringContent("{\"email\":\"toto@titi.com\", \"id\":\"3521\", \"redirectUri\":\"http://localhost\"}");
            A.CallTo(_fakeUserMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Returns(response);
            A.CallTo(() => _authService.RefreshToken(A<string>._, A<string>._, A<DateTime>._)).Returns(null);
            // Act
            var result = await _target.GoogleSignIn(bearer);
            // Assert
            A.CallTo(_fakeTokenMessageHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .MustHaveHappened();
            A.CallTo(() => _authService.RefreshToken("toto@titi.com", "1545151515", A<DateTime>._)).MustHaveHappened();
            Check.That(result as BadRequestObjectResult).IsNotNull();
        }
    }
}
