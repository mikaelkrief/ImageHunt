using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Data;
using ImageHunt.Migrations;
using ImageHuntCore;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NFluent;
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
        private IMapper _mapper;

        private HuntContext _context;
        //private AccountController _target;

        public AccountControllerTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<AccountController>>());
            _testContainerBuilder.RegisterInstance(_userManager = A.Fake<UserManager<Identity>>());
            _testContainerBuilder.RegisterInstance(_signinManager = A.Fake<SignInManager<Identity>>());
            _testContainerBuilder.RegisterInstance(_configuration = A.Fake<IConfiguration>());
            _testContainerBuilder.RegisterInstance(_mapper = A.Fake<IMapper>());

            A.CallTo(() => _configuration["JwtKey"]).Returns("hsjhfdsfsd6767768jsdhfjh");
            A.CallTo(() => _configuration["JwtExpireDays"]).Returns("30");
            A.CallTo(() => _configuration["JwtIssuer"]).Returns("toto");
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HuntContext>()
                .UseSqlite("DataSource=:memory:")
                .EnableSensitiveDataLogging();
            _context = ActivableContext<HuntContext>.CreateInstance(dbContextOptionsBuilder.Options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _context.Database.ExecuteSqlCommand("alter table Nodes add Coordinate point null;");
            _testContainerBuilder.RegisterInstance(_context);
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

        [Fact]
        public async Task Should_Return_Users()
        {
            // Arrange
            var identities = new List<Identity>() {new Identity(), new Identity()};
            A.CallTo(() => _userManager.Users).Returns(identities.AsQueryable());
            // Act
            var result = await _target.GetUsersAsync();
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            Check.That(((OkObjectResult) result).Value).IsInstanceOf<List<UserResponse>>();
        }

        [Fact]
        public async Task Should_Modify_User_Role()
        {
            // Arrange
            var userRequest = new UpdateUserRequest()
            {
                Id = "GHGHG",
                Role = "Admin"
            };
            var identities = new List<Identity>() {new Identity(){Id = "GHGHG" } };
            A.CallTo(() => _userManager.Users).Returns(identities.AsQueryable());
            // Act
            await _target.UpdateUser(userRequest);
            // Assert
            A.CallTo(() => _userManager.AddToRoleAsync(A<Identity>._, userRequest.Role)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_Delete_User_Fail_if_user_not_found()
        {
            // Arrange
            var identities = new List<Identity>() { new Identity() { Id = "GHGHG" } };
            A.CallTo(() => _userManager.Users).Returns(identities.AsQueryable());
            // Act
            var result = await _target.DeleteUser("HHGHGHG");
            // Assert
            Check.That(result).IsInstanceOf<NotFoundObjectResult>();
        }
        [Fact]
        public async Task Should_Delete_User_Succeed()
        {
            // Arrange
            var admin = new Admin() {};
            _context.Admins.Add(admin);
            _context.SaveChanges();
              var identities = new List<Identity>() { new Identity() { Id = "HHGHGHG", AppUserId = admin.Id} };
            A.CallTo(() => _userManager.Users).Returns(identities.AsQueryable());
          // Act
            var result = await _target.DeleteUser("HHGHGHG");
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            Check.That(_context.Admins).HasSize(0);
        }

    }
}
