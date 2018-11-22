using ImageHunt.Model;
using System;
using System.Collections.Generic;
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
    public class AuthServiceTest : ContextBasedTest<HuntContext>
    {
        private AuthService _target;
      private ILogger<AuthService> _logger;

      public AuthServiceTest()
      {
        _logger = A.Fake<ILogger<AuthService>>();
            _target = new AuthService(_context, _logger);
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
