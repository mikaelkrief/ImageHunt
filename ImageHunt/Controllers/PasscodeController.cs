using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using AutoMapper;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class PasscodeController : BaseController
  {
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ITeamService _teamService;
    private readonly IPasscodeService _passcodeService;

    public PasscodeController(IPasscodeService passcodeService, ITeamService teamService, IConfiguration configuration,
      IMapper mapper, UserManager<Identity> userManager) : base(userManager)
    {
      _passcodeService = passcodeService;
      _teamService = teamService;
      _configuration = configuration;
      _mapper = mapper;
    }

    [HttpGet("{gameId}")]
    public IActionResult Get(int gameId)
    {
      var passcodes = _passcodeService.GetAll(gameId);
      var botName = _configuration["BotConfiguration:BotName"];
      var passcodeResponses = _mapper.Map<List<PasscodeResponse>>(passcodes);

      foreach (var passcodeResponse in passcodeResponses)
        //var passcodeResponse = _mapper.Map<PasscodeResponse>(passcode);
        using (var generator = new QRCodeGenerator())
        {
          var payload = $"https://telegram.me/{botName}?start=redeem_gameId={gameId}_pass={passcodeResponse.Pass}";
          using (var code = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H))
          {
            using (var base64QrCode = new Base64QRCode(code))
            {
              var imageStream = Assembly.GetAssembly(GetType())
                .GetManifestResourceStream("ImageHunt.src.assets.ImageHunt.png");
              var image = base64QrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap) Image.FromStream(imageStream),
                30);
              passcodeResponse.QRCode = image;
            }
          }
        }

      return Ok(passcodeResponses);
    }

    [HttpPatch]
    public IActionResult Redeem([FromBody] PasscodeRedeemRequest request)
    {
      var team = _teamService.GetTeamForUserName(request.GameId, request.UserName);
      if (team == null)
        return NotFound(request);
      var redeemStatus = _passcodeService.Redeem(request.GameId, team.Id, request.Pass);
      var passcodeResponse = new PasscodeResponse
      {
        RedeemStatus = redeemStatus
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
    public IActionResult Add([FromBody] PasscodeRequest passcodeRequest)
    {
      var passcode = new Passcode
      {
        NbRedeem = passcodeRequest.NbRedeem,
        Pass = passcodeRequest.Pass,
        Points = passcodeRequest.Points
      };
      return Ok(_passcodeService.Add(passcodeRequest.GameId, passcode));
    }

    [HttpGet("QRCode/{gameId}/{passcodeId}")]
    public IActionResult GetQRCode(int gameId, int passcodeId)
    {
      var passcode = _passcodeService.Get(passcodeId);
      using (var generator = new QRCodeGenerator())
      {
        var botName = _configuration["BotConfiguration:BotName"];
        var payload = $"https://telegram.me/{botName}?start=redeem_gameId={gameId}_pass={passcode.Pass}";
        using (var code = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H))
        {
          using (var base64QrCode = new Base64QRCode(code))
          {
            var imageStream = Assembly.GetAssembly(GetType())
              .GetManifestResourceStream("ImageHunt.src.assets.ImageHunt.png");
            var image = base64QrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap) Image.FromStream(imageStream),
              30);
            return Ok(image);
          }
        }
      }
    }

    [HttpGet("GetPage/{gameId}/{pageNumber}")]
    public IActionResult GetPage(int gameId, int pageNumber)
    {
      var passcodes = _passcodeService.GetAll(gameId);
      // create document
      //var pdfDocument = new PdfDocument();
      return Ok();
    }
  }
}
