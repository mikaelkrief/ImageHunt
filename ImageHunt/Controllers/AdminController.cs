using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  [Authorize]
  public class AdminController : Microsoft.AspNetCore.Mvc.Controller
  {
    private readonly IAdminService _adminService;
    private readonly ILogger _logger;

    public AdminController(IAdminService adminService, ILogger<AdminController> logger)
    {
      _adminService = adminService;
      _logger = logger;
    }
    [HttpGet("GetAllAdmins")]
    public IActionResult GetAllAdmins()
    {
      _logger.LogTrace($"GetAllAdmins");
      return Ok(_adminService.GetAllAdmins());
    }
    [HttpGet("ById/{adminId}")]
    public IActionResult GetAdminById(int adminId)
    {
      return Ok(_adminService.GetAdminById(adminId));
    }
    [HttpGet("ByEmail/{email}")]
    public IActionResult GetAdminByEmail(string email)
    {
      return Ok(_adminService.GetAdminByEmail(email));
    }
    [HttpPost]
    public IActionResult InsertAdmin([FromBody] Admin admin)
    {
      _adminService.InsertAdmin(admin);
      return CreatedAtAction("InsertAdmin", admin);
    }
    [HttpDelete("{adminId}")]
    public IActionResult DeleteAdmin(int adminId)
    {
      var admin = _adminService.GetAdminById(adminId);
      _adminService.DeleteAdmin(admin);
      return Ok();
    }
  }
}
