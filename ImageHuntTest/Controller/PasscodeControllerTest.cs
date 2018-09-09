using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class PasscodeControllerTest
    {
        private PasscodeController _target;
        private IPasscodeService _passcodeService;
        private ITeamService _teamService;

        public PasscodeControllerTest()
        {
            _passcodeService = A.Fake<IPasscodeService>();
            _teamService = A.Fake<ITeamService>();
            _target = new PasscodeController(_passcodeService, _teamService);
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            
            // Act
            var result = _target.Get(1);
            // Assert
            A.CallTo(() => _passcodeService.GetAll(1)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
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
            A.CallTo(() => _teamService.GetTeamForUserName(A<int>._, A<string>._)).Returns(new Team() {Id = 2});
            // Act
            var result = _target.Redeem(new PasscodeRedeemRequest(){GameId = 1, Pass = "ghjgsjdgjhd"});
            // Assert
            A.CallTo(() => _passcodeService.Redeem(1, 2, "ghjgsjdgjhd")).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            Check.That(((OkObjectResult) result).Value).IsInstanceOf<PasscodeResponse>();

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
            var passcodeRequest = new PasscodeRequest() {GameId = 1, Pass = "toto", NbRedeem = 3, Points = 3};
            // Act
            var result = _target.Add(passcodeRequest);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _passcodeService.Add(passcodeRequest.GameId, A<Passcode>.That.Matches(p=>CheckPasscode(p, passcodeRequest)))).MustHaveHappened();
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
    }
}
