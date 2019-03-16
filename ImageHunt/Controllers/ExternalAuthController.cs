using System.Threading.Tasks;
using ImageHuntCore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ExternalAuthController : Controller
  {
    private readonly UserManager<Identity> _userManager;

    public ExternalAuthController(UserManager<Identity> userManager)
    {
      _userManager = userManager;
    }

    public async Task<IActionResult> Google([FromBody] GoogleAuthRequest request)
    {
      return Ok();
    }
  }

  public class GoogleAuthRequest
  {
  }
}
