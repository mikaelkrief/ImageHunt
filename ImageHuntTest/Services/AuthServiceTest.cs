using ImageHunt.Model;
using System;
using System.Collections.Generic;
using ImageHunt.Services;
using NFluent;
using Xunit;

namespace ImageHuntTest.Services
{
    public class AuthServiceTest : ContextBasedTest
    {
        private AuthService _target;

        public AuthServiceTest()
        {
            _target = new AuthService(_context);
        }

        [Fact]
        public void RefreshToken()
        {
            // Arrange
            var admins = new List<Admin>() {new Admin(){Email = "toto@titi.com"},new Admin(){Email = "tato@titi.com"},};
            _context.Admins.AddRange(admins);
            _context.SaveChanges();
            // Act
            _target.RefreshToken("tato@titi.com", "token", DateTime.Now.AddHours(1));
            // Assert
            Check.That(admins[1].Token).Equals("token");
            Check.That(admins[1].ExpirationTokenDate).IsNotNull();
        }
    }
}
