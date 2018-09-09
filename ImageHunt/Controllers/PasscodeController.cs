using System.Linq;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[Controller]")]
  public class PasscodeController : BaseController
  {
    private IPasscodeService _passcodeService;
    private readonly ITeamService _teamService;

    public PasscodeController(IPasscodeService passcodeService, ITeamService teamService)
    {
      _passcodeService = passcodeService;
      _teamService = teamService;
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
  }
}
