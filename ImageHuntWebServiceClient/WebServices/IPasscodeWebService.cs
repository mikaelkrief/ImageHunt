using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IPasscodeWebService
    {
        Task<PasscodeResponse> RedeemPasscode(int gameId, int teamId, string passcode);
    }
}