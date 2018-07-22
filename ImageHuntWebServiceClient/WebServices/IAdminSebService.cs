using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
    public interface IAdminWebService
    {
        Task<IEnumerable<AdminResponse>> GetAllAdmins();
    }
}