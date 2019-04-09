using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Services;
using ImageHuntCore.Model;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class AdminServiceTest : ContextBasedTest<HuntContext>
    {
        private AdminService _target;
      private ILogger<AdminService> _logger;

      public AdminServiceTest()
      {
        _logger = A.Fake<ILogger<AdminService>>();
            _target = new AdminService(Context, _logger);
        }

        [Fact]
        public void GetAllAdmins()
        {
            // Arrange
            
            var admins = new List<Admin>()
            {
                new Admin(){Role = Role.Admin},
                new Admin() {Role = Role.GameMaster},
                new Admin(){Role = Role.Admin}
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAllAdmins();
            // Assert
            Check.That(result).ContainsExactly(admins);
        }
        [Fact]
        public void Should_Get_All_Admin_Return_only_Admin_and_GameMaster()
        {
            // Arrange
            
            var admins = new List<Admin>()
            {
                new Admin(){Role = Role.Admin},
                new Admin() {Role = Role.GameMaster},
                new Admin(){Role = Role.Admin},
                new Admin(){Role = Role.Player},
                new Admin(){Role = Role.Validator},
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAllAdmins();
            // Assert
            Check.That(result).ContainsExactly(admins.Where(a=>a.Role == Role.Admin || a.Role == Role.GameMaster));
        }

        [Fact]
        public void GetAllAdminWithGames()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(),
            };
            admins[0].GameAdmins = new List<GameAdmin>()
            {
                new GameAdmin(){Admin = admins[0], Game = new Game()},
                new GameAdmin(){Admin = admins[0], Game = new Game()},
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAllAdmins();
            // Assert
            Check.That(result.First().Games).ContainsExactly(admins[0].Games);
        }
        [Fact]
        public void InsertAdmin()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(),
                new Admin(),
                new Admin()
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            var admin = new Admin() {Name = "Toto", Email = "toto@titi.com"};
            // Act
            _target.InsertAdmin(admin);
            // Assert
            Check.That(Context.Admins).Contains(admin);
        }

        [Fact]
        public void DeleteAdmin()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(),
                new Admin(),
                new Admin()
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            _target.DeleteAdmin(admins[1]);
            // Assert
            Check.That(Context.Admins).HasSize(2);
        }

        [Fact]
        public void GetAdminById()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(),
                new Admin(),
                new Admin()
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAdminById(2);
            // Assert
            Check.That(result).IsEqualTo(admins[1]);
        }

        [Fact]
        public void GetAdminByEmail()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(){Email = "Toto@titi.com"},
                new Admin(){Email = "Tato@titi.com"},
                new Admin(){Email = "Toto@titi.com" }
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAdminByEmail("Tato@titi.com");
            // Assert
        }
        [Fact]
        public void GetAdminByEmailSomeEmailAreNull()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(){Email = "Toto@titi.com"},
                new Admin(){Email = "Tato@titi.com"},
                new Admin()
            };
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.GetAdminByEmail("Tato@titi.com");
            // Assert
        }

        [Fact]
        public void AssignGame()
        {
            // Arrange
            var games = new List<Game> {new Game(), new Game(), new Game()};
            Context.Games.AddRange(games);
            var admins = new List<Admin> {new Admin(), new Admin()};
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.AssignGame(admins[1].Id, games[1].Id, true);
            // Assert
            Check.That(admins[1].Games).Contains(games[1]);
        }

        [Fact]
        public void AssignGame_RemoveAssignment()
        {
            // Arrange
            var games = new List<Game> { new Game(), new Game(), new Game() };
            Context.Games.AddRange(games);
            var admins = new List<Admin> { new Admin(), new Admin() };
            admins[1].GameAdmins = new List<GameAdmin>(){new GameAdmin(){Admin = admins[1], Game = games[1]}};
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.AssignGame(admins[1].Id, games[1].Id, false);
            // Assert
            Check.That(admins[1].Games).Not.Contains(games[1]);
        }
        [Fact]
        public void AssignGame_Already_assigned()
        {
            // Arrange
            var games = new List<Game> { new Game(), new Game(), new Game() };
            Context.Games.AddRange(games);
            var admins = new List<Admin> { new Admin(), new Admin() };
            admins[1].GameAdmins = new List<GameAdmin>(){new GameAdmin(){Admin = admins[1], Game = games[1]}};
            Context.Admins.AddRange(admins);
            Context.SaveChanges();
            // Act
            var result = _target.AssignGame(admins[1].Id, games[1].Id, true);
            // Assert
            Check.That(admins[1].Games).Contains(games[1]);
        }
    }
}
