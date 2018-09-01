using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class PasscodeControllerTest
    {
        private PasscodeController _target;
        private IPasscodeService _passcodeService;

        public PasscodeControllerTest()
        {
            _passcodeService = A.Fake<IPasscodeService>();
            _target = new PasscodeController(_passcodeService);
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
            
            // Act
            var result = _target.Redeem(new PasscodeRedeemRequest(1, 2, "ghjgsjdgjhd"));
            // Assert
            A.CallTo(() => _passcodeService.Redeem(1, 2, "ghjgsjdgjhd")).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            Check.That(((OkObjectResult) result).Value).IsInstanceOf<RedeemStatus>();

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
    }
}
