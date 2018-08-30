using System.Collections.Generic;
using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public interface IPasscodeService : IService
    {
      IEnumerable<Passcode> GetAll(int gameId);
      RedeemStatus Redeem(int gameId, int teamId, string passcode);
      void Delete(int gameId, int passcodeId);
      Passcode Add(int gameId, Passcode passcode);
    }

  public enum RedeemStatus
  {
    Ok,
    WrongCode,
    FullyRedeem,
    AlreadyRedeem,
  }
}
