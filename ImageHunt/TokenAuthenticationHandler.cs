using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ImageHunt
{
    public class TokenAuthenticationHandler : IAuthenticationHandler
    {
      public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
      {
        throw new NotImplementedException();
      }

      public Task<AuthenticateResult> AuthenticateAsync()
      {
        throw new NotImplementedException();
      }

      public Task ChallengeAsync(AuthenticationProperties properties)
      {
        throw new NotImplementedException();
      }

      public Task ForbidAsync(AuthenticationProperties properties)
      {
        throw new NotImplementedException();
      }
    }
}
