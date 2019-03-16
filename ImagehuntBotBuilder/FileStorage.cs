using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using Polly;
using static Newtonsoft.Json.TypeNameHandling;

namespace ImageHuntBotBuilder
{
    public class FileStorage : IMultiStorage
    {
        private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings
        {
            // we use all so that we get typed roundtrip out of storage, but we don't use validation because we don't know what types are valid
            TypeNameHandling = All
        };

        private static readonly Lazy<Dictionary<char, string>> BadChars = new Lazy<Dictionary<char, string>>(() =>
        {
            var badChars = Path.GetInvalidFileNameChars();
            var dict = new Dictionary<char, string>();
            foreach (var badChar in badChars)
                dict[badChar] = '%' + ((int)badChar).ToString("x2");
            return dict;
        });

        private readonly string _folder;
        private readonly object _syncroot = new object();

        public FileStorage(string folder)
        {
            _folder = folder;
        }

        public async Task<IDictionary<string, object>> ReadAsync(string[] keys,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var storeItems = new Dictionary<string, object>(keys.Length);

            foreach (var key in keys)
            {
                var item = await ReadStoreItemAsync(key);
                if (item != null) storeItems.Add(key, item);
            }

            return storeItems;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> ReadAllAsync(
            CancellationToken cancellationToken = new CancellationToken())
        {
            var keys = await ReadAllKeysAsync();
            var results = new List<IDictionary<string, object>>();
            foreach (var key in keys) results.Add(await ReadAsync(new[] {key }, cancellationToken));

            return results;
        }

        public async Task WriteAsync(IDictionary<string, object> changes,
            CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var change in changes)
            {
                var newValue = change.Value;

                var oldValue = await ReadStoreItemAsync(change.Key);
                var newStoreItem = newValue as IStoreItem;
                var oldStoreItem = oldValue as IStoreItem;

                if (oldValue == null ||
                    newStoreItem?.ETag == "*" ||
                    oldStoreItem?.ETag == newStoreItem?.ETag)
                {
                    var key = SanitizeKey(change.Key);
                    var path = Path.Combine(_folder, key);
                    var oldTag = newStoreItem?.ETag;
                    if (newStoreItem != null)
                        newStoreItem.ETag = Guid.NewGuid().ToString("n");

                    var json = JsonConvert.SerializeObject(newValue, SerializationSettings);
                    await Policy
                        .Handle<IOException>()
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(10, retryAttempt)))
                        .ExecuteAsync(async () =>
                        {
                            using (TextWriter file = new StreamWriter(path))
                            {
                                await file.WriteAsync(json).ConfigureAwait(false);
                            }
                        });
                }
            }
        }

        public async Task DeleteAsync(string[] keys, CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var key in keys) File.Delete(Path.Combine(_folder, key));
        }

        private async Task<IEnumerable<string>> ReadAllKeysAsync()
        {
            return Directory.EnumerateFiles(_folder).Select(f => Path.GetFileName(f));
        }

        private async Task<object> ReadStoreItemAsync(string key)
        {
            key = SanitizeKey(key);
            var path = Path.Combine(_folder, key);
            string json;
            object retVal = null;
            var start = DateTime.UtcNow;
            try
            {
                await Policy
                    .Handle<IOException>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(10, retryAttempt)))
                    .ExecuteAsync(async () =>
                    {
                        using (TextReader file = new StreamReader(File.OpenRead(path)))
                        {
                            json = await file.ReadToEndAsync().ConfigureAwait(false);
                        }

                        retVal = JsonConvert.DeserializeObject(
                            json,
                            SerializationSettings);
                    });
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            return retVal;
        }

        private static string SanitizeKey(string key)
        {
            var sb = new StringBuilder();
            foreach (var ch in key)
                if (BadChars.Value.TryGetValue(ch, out var val))
                    sb.Append(val);
                else
                    sb.Append(ch);

            return sb.ToString();
        }
    }
}