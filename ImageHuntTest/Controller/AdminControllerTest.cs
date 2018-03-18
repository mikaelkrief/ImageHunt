using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class AdminControllerTest
    {
        private IAdminService _adminService;
        private AdminController _target;
      private ILogger<AdminController> _logger;

      public AdminControllerTest()
        {
            _adminService = A.Fake<IAdminService>();
            _logger = A.Fake<ILogger<AdminController>>();
            _target = new AdminController(_adminService, _logger);
        }

        [Fact]
        public void GetAllAdmins()
        {
            // Arrange
            
            // Act
            var result = _target.GetAllAdmins();
            // Assert
            A.CallTo(() => _adminService.GetAllAdmins()).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
        }

        [Fact]
        public void GetAdminById()
        {
            // Arrange
            var admin = new Admin() {Id = 2};
            A.CallTo(() => _adminService.GetAdminById(A<int>._)).Returns(admin);
            // Act
            var result = _target.GetAdminById(2) as OkObjectResult;
            // Assert
            Check.That(result).IsNotNull();
            Check.That(result.Value).Equals(admin);
        }

        [Fact]
        public void GetAdminByEmail()
        {
            // Arrange
            var admin = new Admin() { Id = 2 };
            A.CallTo(() => _adminService.GetAdminByEmail(A<string>._)).Returns(admin);

            // Act
            var result = _target.GetAdminByEmail("toto@titi.com") as OkObjectResult;
            // Assert
            A.CallTo(() => _adminService.GetAdminByEmail(A<string>._)).MustHaveHappened();
            Check.That(result).IsNotNull();
            Check.That(result.Value).IsEqualTo(admin);
        }

        [Fact]
        public void InsertAdmin()
        {
            // Arrange
            var admin = new Admin();
            // Act
            var result = _target.InsertAdmin(admin) as OkResult;
            // Assert
            Check.That(result).IsNotNull();
            A.CallTo(() => _adminService.InsertAdmin(admin)).MustHaveHappened();
        }

        [Fact]
        public void DeleteAdmin()
        {
            // Arrange
            var admin = new Admin();
            A.CallTo(() => _adminService.GetAdminById(A<int>._)).Returns(admin);
            // Act
            var result = _target.DeleteAdmin(2) as OkResult;
            // Assert
            Check.That(result).IsNotNull();
            A.CallTo(() => _adminService.DeleteAdmin(admin)).MustHaveHappened();
        }
    }
}
