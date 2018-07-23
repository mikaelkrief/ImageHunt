using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageHuntTelegramBot
{
  public interface IStorage
  {
    /// <summary>
    /// Read StoreItems from storage
    /// </summary>
    /// <param name="keys">keys of the storeItems to read</param>
    /// <returns>StoreItem dictionary</returns>
    Task<IEnumerable<KeyValuePair<string, object>>> Read(params string[] keys);

    /// <summary>
    /// Write StoreItems to storage
    /// </summary>
    /// <param name="changes"></param>
    Task Write(IEnumerable<KeyValuePair<string, object>> changes);

    /// <summary>
    /// Delete StoreItems from storage
    /// </summary>
    /// <param name="keys">keys of the storeItems to delete</param>
    Task Delete(params string[] keys);
        /// <summary>
        /// Read all StoreItem of a folder
        /// </summary>
        /// <returns></returns>
      Task<IEnumerable<IEnumerable<KeyValuePair<string, object>>>> ReadAll();
  }

  public interface IStoreItem
  {
    /// <summary>
    /// eTag for concurrency
    /// </summary>
    string eTag { get; set; }
  }


  public static class StorageExtensions
  {

    /// <summary>
    /// Storage extension to Read as strong typed StoreItem objects
    /// </summary>
    /// <typeparam name="StoreItemT"></typeparam>
    /// <param name="storage"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<KeyValuePair<string, StoreItemT>>> Read<StoreItemT>(this IStorage storage, params string[] keys) where StoreItemT : class
    {
      var storeItems = await storage.Read(keys).ConfigureAwait(false);

      return ReturnStoreItemsOfDesiredType();

      IEnumerable<KeyValuePair<string, StoreItemT>> ReturnStoreItemsOfDesiredType()
      {
        foreach (var entry in storeItems)
        {
          if (entry.Value is StoreItemT valueAsType)
          {
            yield return new KeyValuePair<string, StoreItemT>(entry.Key, valueAsType);
          }
        }
      }
    }
  }
}