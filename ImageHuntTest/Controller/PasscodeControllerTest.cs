using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
    [Collection("AutomapperFixture")]

    public class PasscodeControllerTest : BaseTest<PasscodeController>
    {
        private IPasscodeService _passcodeService;
        private ITeamService _teamService;
        private IConfiguration _configuration;
        private IMapper _mapper;
        private UserManager<Identity> _userManager;

        public PasscodeControllerTest()
        {
            _testContainerBuilder.RegisterInstance(_passcodeService = A.Fake<IPasscodeService>());
            _testContainerBuilder.RegisterInstance(_teamService = A.Fake<ITeamService>());
            _testContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
            _testContainerBuilder.RegisterInstance(_mapper = Mapper.Instance);
            _testContainerBuilder.RegisterInstance(_userManager = A.Fake<UserManager<Identity>>());
            Build();
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var passcodes = new List<Passcode>() { new Passcode() { Pass = "toto" }, new Passcode() { Pass = "tata" } };
            A.CallTo(() => _passcodeService.GetAll(1)).Returns(passcodes);
            // Act
            var result = _target.Get(1);
            // Assert
            A.CallTo(() => _passcodeService.GetAll(1)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            var response = ((OkObjectResult)result).Value as IEnumerable<PasscodeResponse>;
            Check.That(response.First()).IsInstanceOf<PasscodeResponse>();
        }

        [Fact]
        public void RedemPasscode()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(),
                new Passcode(){Pass = "ghjgsjdgjhd"},
                new Passcode(),
            };
            A.CallTo(() => _passcodeService.GetAll(A<int>._)).Returns(passcodes);
            A.CallTo(() => _teamService.GetTeamForUserName(A<int>._, A<string>._)).Returns(new Team() { Id = 2 });
            // Act
            var result = _target.Redeem(new PasscodeRedeemRequest() { GameId = 1, Pass = "ghjgsjdgjhd" });
            // Assert
            A.CallTo(() => _passcodeService.Redeem(1, 2, "ghjgsjdgjhd")).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            Check.That(((OkObjectResult)result).Value).IsInstanceOf<PasscodeResponse>();

        }

        [Fact]
        public void DeletePasscode()
        {
            // Arrange

            // Act
            var result = _target.Delete(1, 2);
            // Assert
            Check.That(result).IsInstanceOf<OkResult>();
            A.CallTo(() => _passcodeService.Delete(1, 2));
        }

        [Fact]
        public void Add()
        {
            // Arrange
            var passcodeRequest = new PasscodeRequest() { GameId = 1, Pass = "toto", NbRedeem = 3, Points = 3 };
            // Act
            var result = _target.Add(passcodeRequest);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _passcodeService.Add(passcodeRequest.GameId, A<Passcode>.That.Matches(p => CheckPasscode(p, passcodeRequest)))).MustHaveHappened();
        }

        private bool CheckPasscode(Passcode passcode, PasscodeRequest passcodeRequest)
        {
            Check.That(passcode.NbRedeem).Equals(passcodeRequest.NbRedeem);
            Check.That(passcode.Points).Equals(passcodeRequest.Points);
            Check.That(passcode.Pass).Equals(passcodeRequest.Pass);
            return true;
        }

        [Fact]
        public void Redeem_Player_Not_Exists()
        {
            // Arrange
            A.CallTo(() => _teamService.GetTeamForUserName(A<int>._, A<string>._)).Returns(null);
            // Act
            var result = _target.Redeem(new PasscodeRedeemRequest() { GameId = 1, Pass = "ghjgsjdgjhd" });
            // Assert
            Check.That(result).IsInstanceOf<NotFoundObjectResult>();
        }

        [Fact]
        public void Generate_QRCode()
        {
            // Arrange
            A.CallTo(() => _passcodeService.Get(A<int>._)).Returns(new Passcode() { Pass = "toto" });
            A.CallTo(() => _configuration["BotConfiguration:BotName"]).Returns("ImageHuntDevBot");
            // Act
            var result = _target.GetQRCode(1, 15);
            // Assert
            A.CallTo(() => _passcodeService.Get(A<int>._)).MustHaveHappened();
            A.CallTo(() => _configuration["BotConfiguration:BotName"]).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            var image = ((OkObjectResult)result).Value;
            Check.That(image).IsInstanceOf<string>();
        }

        [Fact]
        public void Generate_PDF_Passcode()
        {
            // Arrange
            var passcodes = new List<Passcode>() { new Passcode() { Pass = "Toto" },
                new Passcode() { Pass = "Toto" },
                new Passcode() { Pass = "Toto" }, };
            A.CallTo(() => _passcodeService.GetAll(A<int>._)).Returns(passcodes);
            // Act
            var result = _target.GetPage(1, 1);
            // Assert
            A.CallTo(() => _passcodeService.GetAll(A<int>._)).MustHaveHappened();
        }
    }
}
