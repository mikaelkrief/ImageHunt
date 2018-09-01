using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class PasscodeController : BaseController
  {
    private IPasscodeService _passcodeService;

    public PasscodeController(IPasscodeService passcodeService)
    {
      _passcodeService = passcodeService;
    }
    [HttpGet("{gameId}")]
    public IActionResult Get(int gameId)
    {
      return Ok(_passcodeService.GetAll(gameId));
    }
    [HttpPatch]
    public IActionResult Redeem(PasscodeRedeemRequest request)
    {
      return Ok(_passcodeService.Redeem(request.GameId, request.TeamId, request.Pass));
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
  }
}
