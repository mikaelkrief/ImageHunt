using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
    public class AccountWebService : AbstractWebService, IAccountWebService
    {
        public AccountWebService(HttpClient httpClient, ILogger<IAccountWebService> logger) : base(httpClient, logger)
        {
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jsonString = JsonConvert.SerializeObject(loginRequest);
            using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
            {
                var result = await PostAsync<LoginResponse>($"{_httpClient.BaseAddress}api/Account/Login/",
                    content, cancellationToken);
                return result;
            }
        }
    }
}