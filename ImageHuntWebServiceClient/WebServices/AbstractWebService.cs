using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
  public abstract class AbstractWebService
  {
    protected readonly HttpClient _httpClient;

    public AbstractWebService(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _httpClient.DefaultRequestHeaders.Clear();
    }

    protected async Task<T> GetAsync<T>(string url) where T : class
    {
      var response = await _httpClient.GetAsync(url);
      if (response.IsSuccessStatusCode)
      {
        return await ConvertToObject<T>(response);
      }
      return null;
    }

    private async Task<T> ConvertToObject<T>(HttpResponseMessage response) where T : class
    {
      var responseAsString = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(responseAsString);
    }

    protected async Task PostAsync(string request)
    {
      var result = await _httpClient.PostAsync(request, null);
    }

    protected async Task PutAsync(string request)
    {
      var result = await _httpClient.PutAsync(request, null);
    }

    protected async Task<T> PutAsync<T>(string request) where T : class
    {
      var response = await _httpClient.PutAsync(request, null);
      if (response.IsSuccessStatusCode)
      {
        return await ConvertToObject<T>(response);
      }

      return null;
    }
  }
}