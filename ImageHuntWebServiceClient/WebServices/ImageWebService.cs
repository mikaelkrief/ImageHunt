﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageHuntWebServiceClient.WebServices
{
    public class ImageWebService : AbstractWebService, IImageWebService
    {
        public ImageWebService(HttpClient httpClient, ILogger<IImageWebService> logger) 
            : base(httpClient, logger)
        {
        }

        public async Task<int> UploadImage(Stream imageStream)
        {
            using (var streamContent = new StreamContent(imageStream))
            {
                var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}api/Image/AsStream", streamContent);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var idAdText = await response.Content.ReadAsStringAsync();
                    return Convert.ToInt32(idAdText);
                }

                return 0;
            }
        }
    }
}