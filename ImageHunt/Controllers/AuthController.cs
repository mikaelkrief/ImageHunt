using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageHunt.Controllers
{
  [Route("api/auth")]
  public class AuthController : Controller
  {
    private readonly IConfigurationRoot _configuration;
    private readonly HttpClient _httpToken;
    private readonly HttpClient _httpUser;
    private readonly IAuthService _authService;
    public AuthController(IConfigurationRoot configuration, HttpClient httpToken, HttpClient httpUser, IAuthService authService)
    {
      _configuration = configuration;
      _httpToken = httpToken;
      _httpUser = httpUser;
      _authService = authService;
    }
    [HttpPost("google")]
    public async Task<IActionResult> GoogleSignIn([FromBody]JObject bearer)
    {
      var content = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("code", bearer["code"].Value<string>()),
        new KeyValuePair<string, string>("client_id", _configuration["GoogleApi:ClientId"]),
        new KeyValuePair<string, string>("client_secret", _configuration["GoogleApi:ClientSecret"]),
        new KeyValuePair<string, string>("redirect_uri", bearer["redirectUri"].Value<string>()),
        new KeyValuePair<string, string>("grant_type", "authorization_code"),


      });
      var result = await _httpToken.PostAsync("", content);
      string resultContent = await result.Content.ReadAsStringAsync();
      JObject resultAsObject = JsonConvert.DeserializeObject(resultContent) as JObject;
      var accessToken = resultAsObject["access_token"].Value<string>();
      var userInfo = await _httpUser.GetAsync($"?access_token={accessToken}");
      var userInfoAsString = await userInfo.Content.ReadAsStringAsync();
      var userInfoAsJSon = JsonConvert.DeserializeObject(userInfoAsString) as JObject;
      var email = userInfoAsJSon["email"].Value<string>();
      var expires_in = resultAsObject["expires_in"].Value<int>();
      var user = _authService.RefreshToken(email, accessToken,
        DateTime.Now.AddSeconds(expires_in));
      resultAsObject.Add("email", email);
      if (user == null)
      {
        StatusCode(403);
        return BadRequest("Unknown User");
      }

      return Content(resultAsObject.ToString());

    }
  }
}
