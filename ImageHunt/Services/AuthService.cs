using System;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHuntCore.Services;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
    public class AuthService : AbstractService, IAuthService
    {
        public AuthService(HuntContext context, ILogger<AuthService> logger) : base(context, logger)
        {
        }

        public Admin RefreshToken(string email, string token, DateTime expirationTime)
        {
            var admin = Queryable.Single<Admin>(Context.Admins, a => a.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            admin.Token = token;
            admin.ExpirationTokenDate = expirationTime;
            Context.SaveChanges();
            return admin;
        }
    }
}
