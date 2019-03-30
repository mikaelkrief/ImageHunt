using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImageHunt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImageHunt
{
    public class TokenAuthorizationHandler : IAuthorizationHandler
    {
        private readonly HuntContext _context;

        public TokenAuthorizationHandler(HuntContext context)
        {
            _context = context;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requestHeader = ((AuthorizationFilterContext)context.Resource).HttpContext.Request.Headers["Authorization"];
            if (requestHeader.Count == 0)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var access_token = requestHeader.First().Split(' ')[1];
            var user = _context.Admins.SingleOrDefault(a=>a.Token == access_token && a.ExpirationTokenDate > DateTime.Now);
            var request = ((AuthorizationFilterContext)context.Resource).HttpContext.Request;

            foreach (var authorizationRequirement in context.Requirements)
            {
                if (user !=null)
                {
                  var emailClaim = new Claim("useremail", user.Email);
                  var idClaim = new Claim("userId", user.Id.ToString());
                  context.User.AddIdentity(new ClaimsIdentity(new []
                  {
                    emailClaim,
                    idClaim,
                  }));
                    context.Succeed(authorizationRequirement);
                }
                else
                    context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
