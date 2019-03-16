using System.Linq;
using ImageHuntCore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  public abstract class BaseController : Controller
  {
    private readonly UserManager<Identity> _userManager;

    public BaseController(UserManager<Identity> userManager)
    {
      _userManager = userManager;
    }

    public int UserId
    {
      get
      {
        var claimsIdentity = User.Claims.Single(c => c.Type == new ClaimsIdentityOptions().UserIdClaimType);
        var user = _userManager.Users.Single(u => u.Id == claimsIdentity.Value);
        return user.AppUserId;
      }
    }
  }
}
