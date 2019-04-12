using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageHuntCore
{
    public class AzureBlobProvider : IBlobProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IBlobProvider> _logger;

        public AzureBlobProvider(IConfiguration configuration, ILogger<IBlobProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;
           
        }
        public async Task<string> UploadFromByteArrayAsync(byte[] bytes)
        {
            var creds = CreateStorageCredentials();
            var baseUrl = _configuration["CloudStorage:Url"];
            var container = _configuration["CloudStorage:Container"];
            var url = $"{baseUrl}/{container}/{Path.GetRandomFileName()}";
            var blob = new CloudBlockBlob(new Uri(url), creds);
            if (!(await blob.ExistsAsync()))
            {
                await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            }
            return url;
        }

        private StorageCredentials CreateStorageCredentials()
        {
            var creds = new StorageCredentials(_configuration["CloudStorage:AccountName"],
                _configuration["CloudStorage:AccountKey"]);
            return creds;
        }

        public async Task<byte[]> DownloadToByteArrayAsync(string cloudUrl)
        {
            var creds = CreateStorageCredentials();
            var blob = new CloudBlockBlob(new Uri(cloudUrl), creds) ;
            if (await blob.ExistsAsync())
            {
                var bytes = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(bytes, 0);
                return bytes;
            }

            return null;
        }
    }
}
