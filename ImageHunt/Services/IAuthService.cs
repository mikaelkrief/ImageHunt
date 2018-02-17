using System;
using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public interface IAuthService : IService
    {
      Admin RefreshToken(string email, string token, DateTime expirationTime);
    }
}
