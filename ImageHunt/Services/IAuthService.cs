using System;
using ImageHunt.Model;

namespace ImageHunt.Services
{
    public interface IAuthService : IService
    {
      Admin RefreshToken(string email, string token, DateTime expirationTime);
    }
}
