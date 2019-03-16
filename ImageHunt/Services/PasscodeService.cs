using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHuntCore.Model;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHunt.Services
{
  public class PasscodeService : AbstractService, IPasscodeService
  {
    public PasscodeService(HuntContext context, ILogger<PasscodeService> logger)
      : base(context, logger)
    {
    }

    public IEnumerable<Passcode> GetAll(int gameId)
    {
      return Context.Games.Include(g => g.Passcodes).Single(g => g.Id == gameId).Passcodes;
    }

    public RedeemStatus Redeem(int gameId, int teamId, string pass)
    {
      var game = Context.Games
        .Include(g => g.Passcodes)
        .Include(g => g.Teams).ThenInclude(t => t.TeamPasscodes)
        .Single(g => g.Id == gameId);
      var team = game.Teams.Single(t => t.Id == teamId);
      var passcode = game.Passcodes.SingleOrDefault(p => p.Pass == pass);
      if (passcode == null)
        return RedeemStatus.WrongCode;
      if (passcode.NbRedeem == 0)
        return RedeemStatus.FullyRedeem;
      if (team.Passcodes.Contains(passcode))
        return RedeemStatus.AlreadyRedeem;
      var gameAction = new GameAction
      {
        Action = Action.RedeemPasscode,
        DateOccured = DateTime.Now,
        Game = game,
        Team = team,
        PointsEarned = passcode.Points
      };
      Context.GameActions.Add(gameAction);
      if (passcode.NbRedeem > 0)
        passcode.NbRedeem--;
      team.TeamPasscodes.Add(new TeamPasscode {Passcode = passcode, Team = team});
      Context.SaveChanges();
      return RedeemStatus.Ok;
    }

    public void Delete(int gameId, int passcodeId)
    {
      var game = Context.Games.Include(g => g.Passcodes).Single(g => g.Id == gameId);
      var passcode = game.Passcodes.Single(p => p.Id == passcodeId);
      game.Passcodes.Remove(passcode);
      Context.Passcodes.Remove(passcode);
      Context.SaveChanges();
    }


    public Passcode Add(int gameId, Passcode passcode)
    {
      var game = Context.Games.Include(g => g.Passcodes).Single(g => g.Id == gameId);
      game.Passcodes.Add(passcode);
      Context.Passcodes.Add(passcode);
      Context.SaveChanges();
      return passcode;
    }

    public Passcode Get(int passcodeId)
    {
      return Context.Passcodes.Single(p => p.Id == passcodeId);
    }
  }
}
