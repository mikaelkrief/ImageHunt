using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public class PasscodeWebService : AbstractWebService, IPasscodeWebService
    {
        public PasscodeWebService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<PasscodeResponse> RedeemPasscode(int gameId, int teamId, string passcode)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(gameId.ToString()), "gameId");
                content.Add(new StringContent(teamId.ToString()), "teamId");
                content.Add(new StringContent(passcode), "pass");
                var result = await PatchAsync<PasscodeResponse>($"{_httpClient.BaseAddress}api/Passcode/", content);
                return result;
            };
        }


    }
}