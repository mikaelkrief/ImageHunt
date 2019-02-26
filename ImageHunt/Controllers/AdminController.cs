using System.Collections.Generic;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Bot")]
  #if !DEBUG
  [Authorize]
  #endif
  public class AdminController : BaseController
  {
    private readonly IAdminService _adminService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AdminController(IAdminService adminService, ILogger<AdminController> logger, IMapper mapper, UserManager<Identity> userManager)
      :base(userManager)
    {
      _adminService = adminService;
      _logger = logger;
      _mapper = mapper;
    }
    [HttpGet("GetAllAdmins")]
    public IActionResult GetAllAdmins()
    {
      _logger.LogTrace($"GetAllAdmins");
      var allAdmins = _adminService.GetAllAdmins();
      return Ok(_mapper.Map<IEnumerable<AdminResponse>>(allAdmins));
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
    //[Authorize(Roles = "Admin")]
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
    [HttpPut("Assign/{adminId}/{gameId}")]
    public IActionResult AssignGame(int adminId, int gameId, [FromQuery]bool assign)
    {
      return Ok(_adminService.AssignGame(adminId, gameId, assign));
    }

  }
}
