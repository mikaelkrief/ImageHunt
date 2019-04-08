using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IAccountWebService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default(CancellationToken));
    }
}