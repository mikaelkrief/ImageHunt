using System;
using System.Linq;

namespace ImageHunt.Controllers
{
  public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
  {
    public int UserId
    {
      get
      {
        var claimsIdentity = User.Identities.First(i => i.Claims.Any(c => c.Type == "userId"));
        return Convert.ToInt32(claimsIdentity.Claims.First(c => c.Type == "userId").Value);
      }
    }
  }
}
