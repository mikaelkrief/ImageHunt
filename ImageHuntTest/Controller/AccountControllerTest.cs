using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types.Payments;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class AccountControllerTest : BaseTest<AccountController>
    {
        private ILogger<AccountController> _logger;
        private UserManager<Identity> _userManager;
        private SignInManager<Identity> _signinManager;
        private IConfiguration _configuration;

        public AccountControllerTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<AccountController>>());
            _testContainerBuilder.RegisterInstance(_userManager = A.Fake<UserManager<Identity>>());
            _testContainerBuilder.RegisterInstance(_signinManager = A.Fake<SignInManager<Identity>>());
            _testContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
            A.CallTo(() => _configuration["JwtKey"]).Returns("hsjhfdsfsd6767768jsdhfjh");
            A.CallTo(() => _configuration["JwtExpireDays"]).Returns("30");
            A.CallTo(() => _configuration["JwtIssuer"]).Returns("toto");
            Build();
        }

        [Fact]
        public async Task Should_Register_Succeed()
        {
            // Arrange
            var request = new RegisterRequest()
            {
                Email = "toto@titi.com",
                Password = "toto",
                Login = "toto"
            };
            A.CallTo(() => _userManager.CreateAsync(A<Identity>._, A<string>._)).Returns(IdentityResult.Success);
            // Act
            var result = await _target.Register(request);
            // Assert
            A.CallTo(() => _userManager.CreateAsync(A<Identity>.That.Matches(i => IdentityMatch(i, request.Login, request.Email)), A<string>._))
                .MustHaveHappened();
            A.CallTo(() => _signinManager.SignInAsync(A<Identity>._, A<bool>._, A<string>._)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
        }

        private bool IdentityMatch(Identity identity, string expectedUserName, string expectedEmail)
        {
            Check.That(identity.Email).Equals(expectedEmail);
            Check.That(identity.UserName).Equals(expectedUserName);
            return true;
        }
    }
}
