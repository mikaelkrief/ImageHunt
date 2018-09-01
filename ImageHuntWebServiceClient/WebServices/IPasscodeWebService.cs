using System.Threading.Tasks;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IPasscodeWebService
    {
        Task<RedeemStatus> RedeemPasscode(int gameId, int teamId, string passcode);
    }
}