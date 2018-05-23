using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using NFluent;
using Xunit;

namespace ImageHuntBotTest
{
    public class MemoryStorageTest
    {
      private MemoryStorage _target;

      public MemoryStorageTest()
      {
        _target = new MemoryStorage();
      }
      [Fact]
      public async Task WriteRead()
      {
        // Arrange
        await _target.Write(new[]
        {
          new KeyValuePair<string, object>("key1", new object()), new KeyValuePair<string, object>("key2", new object()),
          new KeyValuePair<string, object>("key3", new object()),
        });
        // Act
        var keyValues = await _target.Read(new[] {"key1", "key2"});
        // Assert
        Check.That(keyValues.Extracting("Key")).Contains("key1", "key2");
      }
      [Fact]
      public async Task WriteWrite()
      {
        // Arrange
        await _target.Write(new[]
        {
          new KeyValuePair<string, object>("key1", new object()), new KeyValuePair<string, object>("key2", new object()),
          new KeyValuePair<string, object>("key3", new object()),
        });
        // Act
        var keyValues = await _target.Read(new[] {"key1", "key2"});
        await _target.Write(keyValues);
        // Assert
        Check.That(keyValues.Extracting("Key")).Contains("key1", "key2");
      }
      [Fact]
      public async Task Delete()
      {
        // Arrange
        await _target.Write(new[]
        {
          new KeyValuePair<string, object>("key1", new object()), new KeyValuePair<string, object>("key2", new object()),
          new KeyValuePair<string, object>("key3", new object()),
        });
        // Act
        await _target.Delete(new[] {"key2"});
      // Assert
      var keyValues = await _target.Read(new[] { "key1", "key2" });
      Check.That(keyValues.Extracting("Key")).Contains("key1");
      }
    }
}
