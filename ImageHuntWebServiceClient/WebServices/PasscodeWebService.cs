using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
    public class PasscodeWebService : AbstractWebService, IPasscodeWebService
    {
        public PasscodeWebService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<PasscodeResponse> RedeemPasscode(int gameId, string userName, string passcode)
        {
            var passcodeRequest = new PasscodeRedeemRequest()
            {
                GameId = gameId,
                UserName = userName,
                Pass = passcode
            };
            using (var content = new StringContent(JsonConvert.SerializeObject(passcodeRequest), Encoding.UTF8,
                "application/json"))
            {
                var result = await PatchAsync<PasscodeResponse>($"{_httpClient.BaseAddress}api/Passcode", content);

                return result;
            }
        }


    }
}