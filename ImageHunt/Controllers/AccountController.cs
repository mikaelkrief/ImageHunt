using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Data;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly IMapper _mapper;
    private readonly HuntContext _context;

    public AccountController(
      UserManager<Identity> userManager,
      SignInManager<Identity> signInManager,
      IConfiguration configuration,
      IMapper mapper,
      HuntContext context
    )
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
      _mapper = mapper;
      _context = context;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

      if (result.Succeeded)
      {
        var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == request.UserName);
        var userRole = _userManager.GetRolesAsync(appUser);
        return Ok(GenerateJwtToken(request.UserName, appUser, userRole));
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
      var admin = new Admin() { Email = request.Email, Name = request.Login, Role = Role.Player};
      _context.Admins.Add(admin);
      _context.SaveChanges();
      user.AppUserId = admin.Id;
      var result = await _userManager.CreateAsync(user, request.Password);

      if (result.Succeeded)
      {
        var identity = _userManager.Users.Single(u => u.Email == request.Email);
        await _userManager.AddToRoleAsync(identity, "Player");
        await _signInManager.SignInAsync(user, false);
        return Ok(user);
      }

      return BadRequest(result);
    }
    private async Task<IActionResult> GenerateJwtToken(string email, Identity user, Task<IList<string>> userRole)
    {
      var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Role, string.Join(",", userRole.Result))
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));
      var token = new JwtSecurityToken(
          _configuration["Jwt:Issuer"],
          _configuration["Jwt:Issuer"],
          claims,
          expires: expires,
          signingCredentials: creds
      );

      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
      var identities = _userManager.Users.AsEnumerable();
      List<UserResponse> users = new List<UserResponse>();
      foreach (var identity in identities)
      {
        var user = _mapper.Map<UserResponse>(identity);
        user.Role = (await _userManager.GetRolesAsync(identity)).FirstOrDefault();
        var admin = _context.Admins
          .Include(a=>a.GameAdmins).ThenInclude(ga=>ga.Game).ThenInclude(g=>g.Picture)
          .Single(a => a.Id == identity.AppUserId);

        user.Games = _mapper.Map<GameResponse[]>(admin.Games.Where(g=>g.IsActive && g.StartDate>=DateTime.Now));
        users.Add(user);
      }


      return Ok(users);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest userRequest)
    {
      var identity = _userManager.Users.Single(u => u.Id == userRequest.Id);
      if (!string.IsNullOrEmpty(userRequest.Role))
      {
        if (!string.IsNullOrEmpty(identity.Role))
          await _userManager.RemoveFromRoleAsync(identity, identity.Role);
        identity.Role = userRequest.Role;
        await _userManager.UpdateAsync(identity);
        await _userManager.AddToRoleAsync(identity, userRequest.Role);
        var userResponse = _mapper.Map<UserResponse>(identity);
        userResponse.Role = userRequest.Role;
        var admin = _context.Admins.Single(a => a.Id == identity.AppUserId);
        admin.Role = Enum.Parse<Role>(userRequest.Role);
        _context.SaveChanges();
        return Ok(userResponse);
      }

      if (!string.IsNullOrEmpty(userRequest.CurrentPassword) && !string.IsNullOrEmpty(userRequest.NewPassword))
      {
        var result = await _userManager.ChangePasswordAsync(identity, userRequest.CurrentPassword, userRequest.NewPassword);
        if (!result.Succeeded)
          return BadRequest(result.Errors);
        return Ok(_mapper.Map<UserResponse>(identity));
      }

      if (!string.IsNullOrEmpty(userRequest.Telegram))
      {
        identity.TelegramUser = userRequest.Telegram;
        var result = await _userManager.UpdateAsync(identity);
        if (!result.Succeeded)
          return BadRequest(result.Errors);
        return Ok(_mapper.Map<UserResponse>(identity));
      }
      return BadRequest();
    }
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
      var identity = _userManager.Users.SingleOrDefault(u => u.Id == userId);
      if (identity == null)
        return NotFound(userId);
      var admin = _context.Admins.SingleOrDefault(a => a.Id == identity.AppUserId);
      _context.Admins.Remove(admin);
      _context.SaveChanges();
      var identityResult = await _userManager.DeleteAsync(identity);
      return Ok(identityResult);
    }
    [HttpGet("{userName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
      var user = _userManager.Users.SingleOrDefault(u => u.UserName == userName);
      if (user == null)
        return NotFound(userName);

      return Ok(_mapper.Map<UserResponse>(user));
    }
  }
}
