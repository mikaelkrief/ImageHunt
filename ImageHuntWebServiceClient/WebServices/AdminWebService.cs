using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public class AdminWebService : AbstractWebService, IAdminWebService
    {
        public AdminWebService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IEnumerable<AdminResponse>> GetAllAdmins()
        {
            var result = await GetAsync<IEnumerable<AdminResponse>>($"{_httpClient.BaseAddress}api/Admin/GetAllAdmins");
            return result;
        }
    }
}