using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace ImageHuntWebServiceClient.WebServices
{
    public abstract class AbstractWebService
    {
        protected readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        protected AbstractWebService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            //_httpClient.DefaultRequestHeaders.Clear();
        }

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return await Policy
                .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
            .ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return await ConvertToObject<T>(response);
                }

                return null;
            });

        }

        protected async Task<T> ConvertToObject<T>(HttpResponseMessage response) where T : class
        {
            var responseContent = response.Content;
            if (responseContent == null)
                return null;
            var responseAsString = await responseContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseAsString);
        }

        protected async Task<T> PostAsync<T>(string request,
                                             HttpContent content,
                                             CancellationToken cancellationToken = default(CancellationToken))
          where T : class
        {
            return await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
                .ExecuteAsync(async () =>
                {

                    var result = await _httpClient.PostAsync(request, content, cancellationToken);
                    if (result.IsSuccessStatusCode)
                    {
                        return await ConvertToObject<T>(result);
                    }

                    return null;
                });

        }

        protected async Task PutAsync(string request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
                .ExecuteAsync(async () =>
                {

                    var result = await _httpClient.PutAsync(request, null, cancellationToken);
                });
        }

        protected async Task<T> PutAsync<T>(string request,
          CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
                .ExecuteAsync(async () =>
                {

                    var response = await _httpClient.PutAsync(request, null, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return await ConvertToObject<T>(response);
                    }

                    return null;
                });
        }

        protected async Task<T> PatchAsync<T>(string uri, HttpContent content, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
                .ExecuteAsync(async () =>
                {
                    var result = await _httpClient.PatchAsync(uri, content, cancellationToken);
                    if (result.IsSuccessStatusCode)
                    {
                        return await ConvertToObject<T>(result);
                    }
                    else
                    {
                        if (result.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new KeyNotFoundException();
                        }
                    }

                    return null;
                });
        }

        protected async Task DeleteAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
                .ExecuteAsync(async () => { await _httpClient.DeleteAsync(uri, cancellationToken); });
        }
    }
}