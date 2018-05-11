using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
  public abstract class AbstractWebService
  {
    protected readonly HttpClient _httpClient;

    protected AbstractWebService(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _httpClient.DefaultRequestHeaders.Clear();
    }

    protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default(CancellationToken)) where T : class
    {
      try
      {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
          return await ConvertToObject<T>(response);
        }

      }
      catch (Exception e)
      {
        return null;
      }

      return null;
    }

    private async Task<T> ConvertToObject<T>(HttpResponseMessage response) where T : class
    {
      var responseAsString = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(responseAsString);
    }

    protected async Task PostAsync<T>(string request, CancellationToken cancellationToken = default (CancellationToken))
    {
      var result = await _httpClient.PostAsync(request, null, cancellationToken);
    }

    protected async Task PutAsync(string request, CancellationToken cancellationToken = default (CancellationToken))
    {
      var result = await _httpClient.PutAsync(request, null, cancellationToken);
    }

    protected async Task<T> PutAsync<T>(string request, 
      CancellationToken cancellationToken = default(CancellationToken) ) where T : class
    {
      var response = await _httpClient.PutAsync(request, null, cancellationToken);
      if (response.IsSuccessStatusCode)
      {
        return await ConvertToObject<T>(response);
      }

      return null;
    }
  }
}