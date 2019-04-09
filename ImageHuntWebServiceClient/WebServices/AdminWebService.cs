using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;

namespace ImageHuntWebServiceClient.WebServices
{
    public class AdminWebService : AbstractWebService, IAdminWebService
    {
        public AdminWebService(HttpClient httpClient, ILogger<IAdminWebService> logger) : base(httpClient, logger)
        {
        }

        public async Task<IEnumerable<AdminResponse>> GetAllAdmins()
        {
            var result = await GetAsync<IEnumerable<AdminResponse>>($"{HttpClient.BaseAddress}api/Admin/GetAllAdmins");
            return result;
        }
    }
}