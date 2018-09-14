using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using SQLitePCL;
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
            _target = new AdminService(_context, _logger);
        }

        [Fact]
        public void GetAllAdmins()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(),
                new Admin(),
                new Admin()
            };
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
            // Act
            var result = _target.GetAllAdmins();
            // Assert
            Check.That(result).ContainsExactly(admins);
        }

        [Fact]
        public void GetAllAdminWithGames()
        {
            // Arrange
            var admins = new List<Admin>()
            {
                new Admin(){Games = new List<Game>() {new Game(), new Game()}},
            };
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
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
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
            var admin = new Admin() {Name = "Toto", Email = "toto@titi.com"};
            // Act
            _target.InsertAdmin(admin);
            // Assert
            Check.That(_context.Admins).Contains(admin);
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
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
            // Act
            _target.DeleteAdmin(admins[1]);
            // Assert
            Check.That(_context.Admins).HasSize(2);
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
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
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
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
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
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
            // Act
            var result = _target.GetAdminByEmail("Tato@titi.com");
            // Assert
        }

    }
}
