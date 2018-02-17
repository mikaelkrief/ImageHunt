using System;
using System.Linq;
using ImageHunt.Model;
using ImageHuntCore.Data;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public class AuthService : AbstractService, IAuthService
    {
        public AuthService(HuntContext context) : base(context)
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
