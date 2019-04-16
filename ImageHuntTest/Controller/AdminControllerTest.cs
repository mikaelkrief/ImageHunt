using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
  [Collection("AutomapperFixture")]
  public class AdminControllerTest : BaseTest<AdminController>
    {
        private IAdminService _adminService;
      private ILogger<AdminController> _logger;
        private IMapper _mapper;
        private UserManager<Identity> _userManager;

        public AdminControllerTest()
        {
            TestContainerBuilder.RegisterInstance(_adminService = A.Fake<IAdminService>());
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<AdminController>>());
            TestContainerBuilder.RegisterInstance(_mapper = AutoMapper.Mapper.Instance);
            TestContainerBuilder.RegisterInstance(_userManager = A.Fake<UserManager<Identity>>());
            Build();
        }

        [Fact]
        public void Should_Admin_Mapped_To_AdminResponse()
        {
            // Arrange
            var admin = new Admin(){Email = "toto@titi.com", Name = "toto", Role = Role.GameMaster};
            admin.GameAdmins = new List<GameAdmin>()
            {
                new GameAdmin() {Game = new Game() {Id = 2}, Admin = admin},
                new GameAdmin() {Game = new Game() {Id = 3}, Admin = admin}
            };
            // Act
            var response = _mapper.Map<AdminResponse>(admin);
            // Assert
            Check.That(response.Name).Equals(admin.Name);
            Check.That(response.Email).Equals(admin.Email);
            Check.That(response.GameIds).Contains(2, 3);
        }
        [Fact]
        public void GetAllAdmins()
        {
            // Arrange
            
            // Act
            var result = Target.GetAllAdmins();
            // Assert
            A.CallTo(() => _adminService.GetAllAdmins()).MustHaveHappened();
          A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
            A<Func<object, Exception, string>>._)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
        }

        [Fact]
        public void GetAdminById()
        {
            // Arrange
            var admin = new Admin() {Id = 2};
            A.CallTo(() => _adminService.GetAdminById(A<int>._)).Returns(admin);
            // Act
            var result = Target.GetAdminById(2) as OkObjectResult;
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
            var result = Target.GetAdminByEmail("toto@titi.com") as OkObjectResult;
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
            var result = Target.InsertAdmin(admin);
            // Assert
            Check.That(result).IsInstanceOf<CreatedAtActionResult>();
            A.CallTo(() => _adminService.InsertAdmin(admin)).MustHaveHappened();
        }

        [Fact]
        public void DeleteAdmin()
        {
            // Arrange
            var admin = new Admin();
            A.CallTo(() => _adminService.GetAdminById(A<int>._)).Returns(admin);
            // Act
            var result = Target.DeleteAdmin(2) as OkResult;
            // Assert
            Check.That(result).IsNotNull();
            A.CallTo(() => _adminService.DeleteAdmin(admin)).MustHaveHappened();
        }

        [Fact]
        public void AssignGameToAdmin()
        {
            // Arrange
            
            // Act
            var response = Target.AssignGame(1, 4, true);
            // Assert
            Check.That(response).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _adminService.AssignGame(A<int>._, A<int>._, true)).MustHaveHappened();
        }
        [Fact]
        public void RemoveGameToAdmin()
        {
            // Arrange
            
            // Act
            var response = Target.AssignGame(1, 4, false);
            // Assert
            Check.That(response).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _adminService.AssignGame(A<int>._, A<int>._, false)).MustHaveHappened();
        }
    }
}
