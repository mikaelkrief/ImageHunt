using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;

namespace ImageHuntWebServiceClient.WebServices
{
    public class AccountWebService : AbstractWebService, IAccountWebService
    {
        public AccountWebService(HttpClient httpClient, ILogger<IAccountWebService> logger) : base(httpClient, logger)
        {
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(loginRequest.UserName), "userName");
                content.Add(new StringContent(loginRequest.Password), "password");
                var result = await PostAsync<LoginResponse>($"{_httpClient.BaseAddress}api/Account/Login/",
                    content, cancellationToken);
                return result;
            }
        }
    }
}