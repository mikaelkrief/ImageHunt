using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class PasscodeController : BaseController
  {
    private IPasscodeService _passcodeService;
    private readonly ITeamService _teamService;
    private readonly IConfiguration _configuration;

    public PasscodeController(IPasscodeService passcodeService, ITeamService teamService, IConfiguration configuration)
    {
      _passcodeService = passcodeService;
      _teamService = teamService;
      _configuration = configuration;
    }
    [HttpGet("{gameId}")]
    public IActionResult Get(int gameId)
    {
      return Ok(_passcodeService.GetAll(gameId));
    }
    [HttpPatch]

    public IActionResult Redeem([FromBody]PasscodeRedeemRequest request)
    {
      var team = _teamService.GetTeamForUserName(request.GameId, request.UserName);
      if (team == null)
        return NotFound(request);
      var redeemStatus = _passcodeService.Redeem(request.GameId, team.Id, request.Pass);
      var passcodeResponse = new PasscodeResponse()
      {
        RedeemStatus = redeemStatus,
      };
      var passcodes = _passcodeService.GetAll(request.GameId);
      var passcode = passcodes.SingleOrDefault(p => p.Pass == request.Pass);
      if (passcode != null)
      {
        passcodeResponse.Id = passcode.Id;
        passcodeResponse.Pass = passcode.Pass;
        passcodeResponse.Points = passcode.Points;
      }
      return Ok(passcodeResponse);
    }
    [HttpDelete("gameId={gameId}&passcodeId={passcodeId}")]
    public IActionResult Delete(int gameId, int passcodeId)
    {
      _passcodeService.Delete(gameId, passcodeId);
      return Ok();
    }
    [HttpPost]
    public IActionResult Add([FromBody]PasscodeRequest passcodeRequest)
    {
      var passcode = new Passcode() { NbRedeem = passcodeRequest.NbRedeem, Pass = passcodeRequest.Pass, Points = passcodeRequest.Points };
      return Ok(_passcodeService.Add(passcodeRequest.GameId, passcode));
    }
    [HttpGet("QRCode/{gameId}/{passcodeId}")]
    public IActionResult GetQRCode(int gameId, int passcodeId)
    {
      var passcode = _passcodeService.Get(passcodeId);
      using (var generator = new QRCodeGenerator())
      {
        var botName = _configuration["BotConfiguration.BotName"];
        var payload = $"https://telegram.me/{botName}?start=redeem_gameId={gameId}_pass={passcode.Pass}";
        using (var code = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H))
        {
          using (var base64Code = new Base64QRCode(code))
          {
            var imageStream = Assembly.GetAssembly(this.GetType())
              .GetManifestResourceStream("ImageHunt.src.assets.ImageHunt.png");
            return Ok(base64Code.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromStream(imageStream), 30));
          }
        }

      }
    }
  }
}
