using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImageHunt.Data;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : Controller
    {
      private readonly UserManager<Identity> _userManager;
      private readonly SignInManager<Identity> _signInManager;
      private readonly IConfiguration _configuration;
      private readonly HuntContext _context;

      public AccountController(
        UserManager<Identity> userManager,
        SignInManager<Identity> signInManager,
        IConfiguration configuration,
        HuntContext context
      )
      {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
      }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

      if (result.Succeeded)
      {
        var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == request.UserName);
        return Ok(await GenerateJwtToken(request.UserName, appUser));
      }

      return BadRequest(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
      var user = new Identity
      {
        UserName = request.Login,
        Email = request.Email,
        TelegramUser = request.Telegram
      };
      var admin = new Admin() {Email = request.Email, Name = request.Login};
      _context.Admins.Add(admin);
      _context.SaveChanges();
      user.AppUserId = admin.Id;
      var result = await _userManager.CreateAsync(user, request.Password);

      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false);
        return Ok("Account created");
      }

      return BadRequest(result);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("Protected")]
      public async Task<IActionResult> Protected()
      {
        return Ok("Protected area");
      }
    private async Task<IActionResult> GenerateJwtToken(string email, IdentityUser user)
    {
      var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
      var token = new JwtSecurityToken(
          _configuration["JwtIssuer"],
          _configuration["JwtIssuer"],
          claims,
          expires: expires,
          signingCredentials: creds
      );

      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
  }
}
